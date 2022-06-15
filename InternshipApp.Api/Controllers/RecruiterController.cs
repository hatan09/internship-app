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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RecruiterController : ControllerBase
    {
        private readonly RecruiterManager _recruiterManager;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public RecruiterController(RecruiterManager recruiterManager, ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _recruiterManager = recruiterManager;
            _mapper = mapper;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var recruiter = await _recruiterManager.FindByIdAsync(id);
            if (recruiter is null || recruiter.IsDeleted)
                return NotFound();

            return Ok(_mapper.Map<RecruiterDTO>(recruiter));
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RecruiterDTO dto, CancellationToken cancellationToken = default)
        {
            var company = await _companyRepository.FindByIdAsync(dto.CompanyId);
            if(company is null) return NotFound(" No Company Found ");

            var recruiter = _mapper.Map<Recruiter>(dto);
            recruiter.Company = company;

            var result = await _recruiterManager.CreateAsync(recruiter, dto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            // Add user to specified roles
            var addtoRoleResullt = await _recruiterManager.AddToRoleAsync(recruiter, "recruiter");
            if (!addtoRoleResullt.Succeeded)
            {
                return BadRequest("Fail to add role");
            }

            return CreatedAtAction(nameof(Get), new { recruiter.Id }, _mapper.Map<RecruiterDTO>(recruiter));
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RecruiterDTO dto)
        {
            var recruiter = await _recruiterManager.FindByIdAsync(dto.Id);
            if (recruiter is null || recruiter.IsDeleted)
                return NotFound();

            _mapper.Map(dto, recruiter);
            await _recruiterManager.UpdateAsync(recruiter);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var recruiter = await _recruiterManager.FindByIdAsync(id);
            if (recruiter is null || recruiter.IsDeleted)
                return NotFound();

            recruiter.IsDeleted = true;
            await _recruiterManager.UpdateAsync(recruiter);
            return NoContent();
        }
    }
}
