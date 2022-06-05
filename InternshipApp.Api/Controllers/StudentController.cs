using AutoMapper;
using InternshipApp.Api.DataObjects;
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
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IJobRepository _jobGroupRepository;
        private readonly IInternGroupRepository _internGroupRepository;
        private readonly StudentManager _studentManager;
        private readonly IMapper _mapper;

        public StudentController(IJobRepository jobGroupRepository, IInternGroupRepository internGroupRepository, StudentManager studentManager, IMapper mapper)
        {
            _jobGroupRepository = jobGroupRepository;
            _internGroupRepository = internGroupRepository;
            _studentManager = studentManager;
            _mapper = mapper;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var student = await _studentManager.FindByIdAsync(id);
            if (student is null || student.IsDeleted)
                return NotFound();

            return Ok(_mapper.Map<StudentDTO>(student));
        }


        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetAllByGroup(int groupId, CancellationToken cancellationToken = default)
        {
            var group = await _internGroupRepository.FindByIdAsync(groupId, cancellationToken);
            if(group is null) return NotFound("No Intern Group Found!");

            var students = await _studentManager.FindAll().Where(stu => stu.InternGroupId == groupId).Select(stu => new
            {
                StudentJobs = stu.StudentJobs.Where(sj => sj.IsAccepted)
            })
                .ToListAsync(cancellationToken);
            if(students is null) return NotFound("No Student Found");

            return Ok(_mapper.Map<IEnumerable<StudentDTO>>(students));

        }


        [HttpGet("{jobId}")]
        public async Task<IActionResult> GetAllByJob(int jobId, CancellationToken cancellationToken = default)
        {
            var job = await _jobGroupRepository.FindAll().Where(job => job.Id == jobId).Include(job => job.StudentJobs).ThenInclude(sj => sj.Student).FirstOrDefaultAsync(cancellationToken);
            if (job is null) return NotFound("No Job Found!");

            if (job.StudentJobs is null) return NotFound("No Student Found");

            var students = job.StudentJobs.Select(sj => sj.Student);

            return Ok(_mapper.Map<IEnumerable<StudentDTO>>(students));

        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudentDTO dto, CancellationToken cancellationToken = default)
        {
            
            var student = await _studentManager.FindByStudentId(dto.StudentId, cancellationToken);
            if (student is not null) return NotFound("Student exist");

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
