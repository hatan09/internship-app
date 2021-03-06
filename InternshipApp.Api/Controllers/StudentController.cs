using AutoMapper;
using InternshipApp.Api.DataObjects;
using InternshipApp.Api.Models;
using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace InternshipApp.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IJobRepository _jobRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly IInternGroupRepository _internGroupRepository;
        private readonly StudentManager _studentManager;
        private readonly IMapper _mapper;

        public StudentController(IJobRepository jobRepository, ISkillRepository skillRepository, IInternGroupRepository internGroupRepository, StudentManager studentManager, IMapper mapper)
        {
            _skillRepository = skillRepository;
            _jobRepository = jobRepository;
            _internGroupRepository = internGroupRepository;
            _studentManager = studentManager;
            _mapper = mapper;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id, CancellationToken cancellationToken = default)
        {
            var student = await _studentManager
                .FindAll(stu => stu.Id.Equals(id))
                .Include(stu => stu.StudentSkills)
                .Include(stu => stu.StudentJobs)
                .FirstOrDefaultAsync(cancellationToken);
            if (student is null || student.IsDeleted)
                return NotFound();

            return Ok(_mapper.Map<GetStudentDTO>(student));
        }


        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetRegister(int groupId, CancellationToken cancellationToken = default)
        {
            var group = await _internGroupRepository.FindByIdAsync(groupId, cancellationToken);
            if(group is null) return NotFound("No Intern Group Found");

            var students = await _studentManager.FindAll().Where(stu => stu.InternGroupId == groupId && stu.Stat == Stat.PENDING).ToListAsync(cancellationToken);
            if(students is null) return NotFound("No Student Found");

            return Ok(_mapper.Map<IEnumerable<StudentDTO>>(students));

        }


        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetAllByGroup(int groupId, CancellationToken cancellationToken = default)
        {
            var group = await _internGroupRepository.FindByIdAsync(groupId, cancellationToken);
            if (group is null) return NotFound("No Intern Group Found");

            var students = await _studentManager.FindAll()
                .Where(stu => stu.InternGroupId == groupId && stu.Stat == Stat.ACCEPTED)
                .Include(stu => stu.StudentJobs)
                .ToListAsync(cancellationToken);
            if (students is null) return NotFound("No Student Found");

            return Ok(_mapper.Map<IEnumerable<GetStudentDTO>>(students));
        }


        [HttpGet("{jobId}")]
        public async Task<IActionResult> GetAllByJob(int jobId, CancellationToken cancellationToken = default)
        {
            var job = await _jobRepository.FindAll().Where(job => job.Id == jobId).Include(job => job.StudentJobs!).ThenInclude(sj => sj.Student).FirstOrDefaultAsync(cancellationToken);
            if (job is null) return NotFound("No Job Found!");

            if (job.StudentJobs is null) return NotFound("No Student Found");

            var students = job.StudentJobs.Select(sj => sj.Student);

            return Ok(_mapper.Map<IEnumerable<GetStudentDTO>>(students));

        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudentDTO dto, CancellationToken cancellationToken = default)
        {
            var student = await _studentManager.FindByStudentId(dto.StudentId, cancellationToken);
            if (student is not null) return BadRequest("Student exist");

            var group = await _internGroupRepository.FindByDepartment(dto.DepartmentId!.Value).FirstOrDefaultAsync(cancellationToken);
            if (group is null) return NotFound();

            student = _mapper.Map<Student>(dto);
            student.InternGroupId = group!.Id;

            var result = await _studentManager.CreateAsync(student, dto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            // Add user to specified roles
            var addtoRoleResullt = await _studentManager.AddToRoleAsync(student, "student");
            if (!addtoRoleResullt.Succeeded)
            {
                return BadRequest("Fail to add role");
            }

            return CreatedAtAction(nameof(Get), new { student.Id }, _mapper.Map<StudentDTO>(student));
        }


        [HttpPut]
        public async Task<IActionResult> AddSkill([FromBody] AddSkillModel model)
        {
            var student = await _studentManager.FindByIdAsync(model.StudentId);
            if (student is null || student.IsDeleted)
                return NotFound("No Student Found");

            foreach(var id in model.Skills)
            {
                var skill = await _skillRepository.FindByIdAsync(id);
                if (skill is not null)
                {
                    student.StudentSkills.Add(new StudentSkill { Student = student, StudentId = student.Id, Skill = skill, SkillId = id, Level = Level.BEGINNER});
                }
            }

            await _studentManager.UpdateAsync(student);
            return NoContent();
        }


        [HttpPut]
        public async Task<IActionResult> Apply([FromBody] ApplyJobModel model)
        {
            var student = await _studentManager.FindByIdAsync(model.StudentId);
            if (student is null || student.IsDeleted || student.Stat == Stat.PENDING)
                return NotFound();

            var job = await _jobRepository.FindByIdAsync(model.JobId);
            if (job is null)
                return NotFound("No Job Found");

            student.StudentJobs.Add(new StudentJob { Student = student, StudentId = model.StudentId, Job = job, JobId = model.JobId, IsAccepted = false });

            await _studentManager.UpdateAsync(student);
            return NoContent();
        }


        [HttpPut]
        public async Task<IActionResult> Accept([FromBody] UpdateStudentJobModel model, CancellationToken cancellationToken = default)
        {
            var student = await _studentManager.FindAll(stu => stu.Id.Equals(model.StudentId)).Include(stu => stu.StudentJobs).FirstAsync(cancellationToken);
            if (student is null || student.IsDeleted || student.Stat == Stat.PENDING || student.Stat == Stat.DENIED)
                return NotFound("No Student Found");

            if (student.StudentJobs is null) return NotFound();

            var sj = student.StudentJobs!.First(sj => sj.JobId == model.JobId);
            if (sj is null) return NotFound("Student Has Not Applied For That Job");

            student.StudentJobs!.First(sj => sj.JobId == model.JobId).IsAccepted = true;

            await _studentManager.UpdateAsync(student);
            return NoContent();
        }


        [HttpPut]
        public async Task<IActionResult> Reject([FromBody] UpdateStudentJobModel model, CancellationToken cancellationToken = default)
        {
            var student = await _studentManager.FindAll(stu => stu.Id.Equals(model.StudentId)).Include(stu => stu.StudentJobs).FirstAsync(cancellationToken);
            if (student is null || student.IsDeleted || student.Stat == Stat.PENDING || student.Stat == Stat.DENIED)
                return NotFound("No Student Found");

            if (student.StudentJobs is null) return NotFound();

            var sj = student.StudentJobs!.First(sj => sj.JobId == model.JobId);
            if (sj is null) return NotFound("Student Has Not Applied For That Job");

            student.StudentJobs!.Remove(sj);

            await _studentManager.UpdateAsync(student);
            return NoContent();
        }


        [HttpPut]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusModel model)
        {
            var student = await _studentManager.FindByIdAsync(model.Id);
            if (student is null || student.IsDeleted)
                return NotFound();

            student.Stat = model.Stat;
            await _studentManager.UpdateAsync(student);
            return NoContent();
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] StudentDTO dto)
        {
            var student = await _studentManager.FindByIdAsync(dto.Id);
            if (student is null || student.IsDeleted)
                return NotFound();

            _mapper.Map(dto, student);
            await _studentManager.UpdateAsync(student);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            var student = await _studentManager.FindByIdAsync(id);
            if (student is null || student.IsDeleted)
                return NotFound();

            student.IsDeleted = true;
            await _studentManager.UpdateAsync(student);
            return NoContent();
        }
    }
}
