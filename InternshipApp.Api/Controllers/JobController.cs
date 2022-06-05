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
    public class JobController : ControllerBase
    {
        private readonly ISkillRepository _skillRepository;
        private readonly IJobRepository _jobRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public JobController(ISkillRepository skillRepository, IJobRepository jobRepository, IDepartmentRepository departmentRepository, ICompanyRepository companyRepository, IMapper mapper)
        {
            _skillRepository = skillRepository;
            _jobRepository = jobRepository;
            _departmentRepository = departmentRepository;
            _companyRepository = companyRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var skills = await _skillRepository.FindAll().ToListAsync(cancellationToken);

            return Ok(_mapper.Map<IEnumerable<SkillDTO>>(skills));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var job = await _jobRepository.FindByIdAsync(id);
            if (job is null)
                return NotFound("No Job Found");

            return Ok(_mapper.Map<JobDTO>(job));
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateJobDTO dto, CancellationToken cancellationToken = default)
        {
            var job = _mapper.Map<Job>(dto);

            var company = await _companyRepository.FindByIdAsync(dto.CompanyId, cancellationToken);
            if(company is null) return NotFound("No Company Found");

            var test = dto.DepartmentIds;

            //var test2 = _companyRepository.FindAll(com => test.Contains(com.Id));

            foreach(var js in dto.JobSkills)
            {
                var skill = await _skillRepository.FindByIdAsync(js.SkillId, cancellationToken);
                if (skill is not null) job.JobSkills.Add(js);
            }

            foreach (var id in dto.DepartmentIds)
            {
                var department = await _departmentRepository.FindByIdAsync(id, cancellationToken);
                if (department is not null) job.Departments.Add(department);
            }

            job.Company = company;

            _jobRepository.Add(job);
            await _skillRepository.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(nameof(Get), new { job.Id }, _mapper.Map<JobDTO>(job));
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] JobDTO dto, CancellationToken cancellationToken = default)
        {
            var job = await _jobRepository.FindByIdAsync(dto.Id, cancellationToken);
            if (job is null)
                return NotFound();

            _mapper.Map(dto, job);
            _jobRepository.Update(job);
            await _jobRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var job = await _jobRepository.FindByIdAsync(id);
            if (job is null)
                return NotFound("No Job Found");

            _jobRepository.Delete(job);
            await _jobRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}
