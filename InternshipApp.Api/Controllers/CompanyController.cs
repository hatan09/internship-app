using AutoMapper;
using InternshipApp.Api.DataObjects;
using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternshipApp.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompanyController(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var companies = await _companyRepository.FindAll().ToListAsync(cancellationToken);

            return Ok(_mapper.Map<IEnumerable<CompanyDTO>>(companies));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var company = await _companyRepository.FindByIdAsync(id);
            if (company is null)
                return NotFound();

            return Ok(_mapper.Map<CompanyDTO>(company));
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CompanyDTO dto, CancellationToken cancellationToken = default)
        {
            var company = _mapper.Map<Company>(dto);

            _companyRepository.Add(company);
            await _companyRepository.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(nameof(Get), new { company.Id }, _mapper.Map<CompanyDTO>(company));
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CompanyDTO dto, CancellationToken cancellationToken = default)
        {
            var company = await _companyRepository.FindByIdAsync(dto.Id, cancellationToken);
            if (company is null)
                return NotFound("No Company Found");

            _mapper.Map(dto, company);
            _companyRepository.Update(company);
            await _companyRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.FindByIdAsync(id, cancellationToken);
            if (company is null)
                return NotFound("No Company Found");

            _companyRepository.Delete(company);
            await _companyRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}
