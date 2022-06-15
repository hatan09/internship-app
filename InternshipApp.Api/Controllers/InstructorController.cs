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
    public class InstructorController : ControllerBase
    {
        private readonly InstructorManager _instructorManager;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public InstructorController(InstructorManager instructorManager, IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _instructorManager = instructorManager;
            _mapper = mapper;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var instructor = await _instructorManager.FindByIdAsync(id);
            if (instructor is null || instructor.IsDeleted)
                return NotFound();

            return Ok(_mapper.Map<InstructorDTO>(instructor));
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InstructorDTO dto, CancellationToken cancellationToken = default)
        {
            var dep = await _departmentRepository.FindByIdAsync(dto.DepartmentId, cancellationToken);
            if (dep is null) return NotFound("No Department Found");

            var instructor = _mapper.Map<Instructor>(dto);
            instructor.Department = dep;

            var result = await _instructorManager.CreateAsync(instructor, dto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            // Add user to specified roles
            var addtoRoleResullt = await _instructorManager.AddToRoleAsync(instructor, "instructor");
            if (!addtoRoleResullt.Succeeded)
            {
                return BadRequest("Fail to add role");
            }

            return CreatedAtAction(nameof(Get), new { instructor.Id }, _mapper.Map<InstructorDTO>(instructor));
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] InstructorDTO dto)
        {
            var instructor = await _instructorManager.FindByIdAsync(dto.Id);
            if (instructor is null || instructor.IsDeleted)
                return NotFound();

            _mapper.Map(dto, instructor);
            await _instructorManager.UpdateAsync(instructor);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            var instructor = await _instructorManager.FindByIdAsync(id);
            if (instructor is null || instructor.IsDeleted)
                return NotFound();

            instructor.IsDeleted = true;
            await _instructorManager.UpdateAsync(instructor);
            return NoContent();
        }
    }
}
