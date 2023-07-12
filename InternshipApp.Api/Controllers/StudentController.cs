using AutoMapper;
using InternshipApp.Api.DataObjects;
using InternshipApp.Api.Models;
using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                .FindAll(stu => stu.Id == id)
                .Include(stu => stu.StudentSkills)
                .Include(stu => stu.StudentJobs)
                .FirstOrDefaultAsync(cancellationToken);
            if (student is null || student.IsDeleted)
                return NotFound();

            return Ok(_mapper.Map<GetStudentDTO>(student));
        }


        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetAllByGroup(int groupId, CancellationToken cancellationToken = default)
        {
            var group = await _internGroupRepository.FindByIdAsync(groupId, cancellationToken);
            if (group is null) return NotFound("No Intern Group Found");

            var students = await _studentManager.FindAll()
                .Where(stu => stu.InternGroupId == groupId)
                .Include(stu => stu.StudentJobs)
                .ToListAsync(cancellationToken);
            if (students is null) return NotFound("No Student Found");

            return Ok(_mapper.Map<IEnumerable<GetStudentDTO>>(students));
        }


        [HttpGet]
        public async Task<IActionResult> GetAllWithoutGroup(CancellationToken cancellationToken = default)
        {
            var students = await _studentManager.FindAll()
                .Where(stu => stu.InternGroup == null)
                .ToListAsync(cancellationToken);
            if (students is null || students.Count == 0) return NotFound("No Student Found");

            return Ok(_mapper.Map<IEnumerable<GetStudentDTO>>(students));
        }


        [HttpGet("{jobId}")]
        public async Task<IActionResult> GetAllHireByJob(int jobId, CancellationToken cancellationToken = default)
        {
            var job = await _jobRepository.FindAll()
                .Where(job => job.Id == jobId)
                .Include(job => job.StudentJobs!.Where(x => x.Status == ApplyStatus.HIRED))
                .ThenInclude(sj => sj.Student)
                .FirstOrDefaultAsync(cancellationToken);
            if (job is null) return NotFound("No Job Found!");

            if (job.StudentJobs is null) return NotFound("No Student Found");

            var students = job.StudentJobs.Select(sj => sj.Student);

            return Ok(_mapper.Map<IEnumerable<GetStudentDTO>>(students));
        }


        [HttpGet("{jobId}")]
        public async Task<IActionResult> GetAllApplicantByJob(int jobId, CancellationToken cancellationToken = default)
        {
            var job = await _jobRepository.FindAll()
                .Where(job => job.Id == jobId)
                .Include(job => job.StudentJobs!.Where(x => x.Status != ApplyStatus.HIRED))
                .ThenInclude(sj => sj.Student)
                .FirstOrDefaultAsync(cancellationToken);
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

            student = _mapper.Map<Student>(dto);

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
        public async Task<IActionResult> AddSkill([FromBody] AddSkillModel model, CancellationToken cancellationToken = default)
        {
            var student = await _studentManager.FindAll(x => x.Id == model.StudentId).Include(x => x.StudentSkills).FirstOrDefaultAsync(cancellationToken);
            if (student is null || student.IsDeleted)
                return NotFound("No Student Found");

            foreach (var id in model.Skills)
            {
                var skill = await _skillRepository.FindByIdAsync(id, cancellationToken);
                if (skill is not null)
                {
                    student.StudentSkills?.Add(new StudentSkill { Student = student, StudentId = student.Id, Skill = skill, SkillId = id });
                }
            }

            await _studentManager.UpdateAsync(student);
            return NoContent();
        }


        [HttpPut]
        public async Task<IActionResult> Apply([FromBody] ApplyJobModel model)
        {
            var student = await _studentManager.FindByIdAsync(model.StudentId);
            if (student is null || student.IsDeleted || student.Stat == Stat.HIRED)
                return NotFound("No Student Found");

            if (student.Stat == Stat.ACCEPTED)
                return BadRequest("Student got an intern job");

            var job = await _jobRepository.FindByIdAsync(model.JobId);
            if (job is null)
                return NotFound("No Job Found");

            student.StudentJobs.Add(new StudentJob { Student = student, StudentId = model.StudentId, Job = job, JobId = model.JobId });

            await _studentManager.UpdateAsync(student);
            return NoContent();
        }


        [HttpPut]
        public async Task<IActionResult> Accept([FromBody] UpdateStudentJobModel model, CancellationToken cancellationToken = default)
        {
            var student = await _studentManager.FindAll(stu => stu.Id.Equals(model.StudentId)).Include(stu => stu.StudentJobs).FirstAsync(cancellationToken);
            if (student is null || student.IsDeleted || student.Stat == Stat.HIRED)
                return NotFound("No Student Found");

            if (student.StudentJobs is null) return NotFound();

            var sj = student.StudentJobs!.First(sj => sj.JobId == model.JobId);
            if (sj is null) return NotFound("Student Has Not Applied For That Job");

            student.StudentJobs!.First(sj => sj.JobId == model.JobId).Status = ApplyStatus.ACCEPTED;

            await _studentManager.UpdateAsync(student);
            return NoContent();
        }


        [HttpPut]
        public async Task<IActionResult> Hire([FromBody] UpdateStudentJobModel model, CancellationToken cancellationToken = default)
        {
            var student = await _studentManager.FindAll(stu => stu.Id.Equals(model.StudentId)).Include(stu => stu.StudentJobs).FirstAsync(cancellationToken);
            if (student is null || student.IsDeleted || student.Stat == Stat.HIRED)
                return NotFound("No Student Found");

            if (student.StudentJobs is null) return NotFound();

            var sj = student.StudentJobs!.First(sj => sj.JobId == model.JobId);
            if (sj is null) return NotFound("Student Has Not Applied For That Job");

            student.StudentJobs!.First(sj => sj.JobId == model.JobId).Status = ApplyStatus.HIRED;

            await _studentManager.UpdateAsync(student);
            return NoContent();
        }


        [HttpPut]
        public async Task<IActionResult> Reject([FromBody] UpdateStudentJobModel model, CancellationToken cancellationToken = default)
        {
            var student = await _studentManager.FindAll(stu => stu.Id.Equals(model.StudentId)).Include(stu => stu.StudentJobs).FirstAsync(cancellationToken);
            if (student is null || student.IsDeleted)
                return NotFound("No Student Found");

            if (student.StudentJobs is null) return NotFound();

            var sj = student.StudentJobs?.FirstOrDefault(sj => sj.JobId == model.JobId);
            if (sj is null) return NotFound("Student Has Not Applied For That Job");

            student.StudentJobs!.First(sj => sj.JobId == model.JobId).Status = ApplyStatus.REJECTED;

            await _studentManager.UpdateAsync(student);
            return NoContent();
        }


        [HttpPut("{studentId}")]
        public async Task<IActionResult> Disqualify(string studentId, CancellationToken cancellationToken = default)
        {
            var student = await _studentManager.FindAll(stu => stu.Id == studentId).Include(stu => stu.StudentJobs).Include(x => x.InternGroup).FirstOrDefaultAsync(cancellationToken);
            if (student is null || student.IsDeleted)
                return NotFound("No Student Found");

            if (student.StudentJobs != null && student.StudentJobs.Any())
            {
                student.StudentJobs = null;
            }
            student.InternGroup = null;
            student.Stat = Stat.REJECTED;

            await _studentManager.UpdateAsync(student);
            return NoContent();
        }


        [HttpPut("{studentId}")]
        public async Task<IActionResult> FinishIntern(string studentId, CancellationToken cancellationToken = default)
        {
            var student = await _studentManager.FindAll(stu => stu.Id == studentId).FirstOrDefaultAsync(cancellationToken);
            if (student is null || student.IsDeleted)
                return NotFound("No Student Found");

            student.Stat = Stat.FINISHED;

            await _studentManager.UpdateAsync(student);
            return NoContent();
        }


        [HttpPut]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusModel model)
        {
            var student = await _studentManager.FindByIdAsync(model.Id);
            if (student is null || student.IsDeleted)
                return NotFound("No Student Found");

            student.Stat = model.Stat;
            await _studentManager.UpdateAsync(student);
            return NoContent();
        }


        [HttpPut]
        public async Task<IActionResult> UpdateSkill([FromBody] UpdateSkillModel model, CancellationToken cancellationToken = default)
        {
            var student = await _studentManager.FindAll(x => x.Id == model.StudentId).Include(x => x.StudentSkills).FirstOrDefaultAsync(cancellationToken);
            if (student is null || student.IsDeleted)
                return NotFound("No Student Found");

            var notRelatedSkills = student.StudentSkills?.Where(x => !model.Skills.Select(x => x.SkillId).Contains(x.SkillId ?? 0)).ToList() ?? new();

            foreach (var skill in model.Skills)
            {
                notRelatedSkills.Add(new()
                {
                    SkillId = skill.SkillId,
                    StudentId = student.Id,
                    Description = skill.Description
                });
            }
            student.StudentSkills = notRelatedSkills;
            await _studentManager.UpdateAsync(student);
            return NoContent();
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] StudentDTO dto)
        {
            var student = await _studentManager.FindByIdAsync(dto.Id);
            if (student is null || student.IsDeleted)
                return NotFound("No Student Found");

            _mapper.Map(dto, student);
            await _studentManager.UpdateAsync(student);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            var student = await _studentManager.FindByIdAsync(id);
            if (student is null || student.IsDeleted)
                return NotFound("No Student Found");

            student.IsDeleted = true;
            await _studentManager.UpdateAsync(student);
            return NoContent();
        }
    }
}
