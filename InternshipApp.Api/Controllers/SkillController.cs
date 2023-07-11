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
    public class SkillController : ControllerBase
    {
        private readonly ISkillRepository _skillRepository;
        private readonly IMapper _mapper;

        public SkillController(ISkillRepository skillRepository, IMapper mapper)
        {
            _skillRepository = skillRepository;
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
            var skill = await _skillRepository.FindByIdAsync(id);
            if (skill is null)
                return NotFound();

            return Ok(_mapper.Map<SkillDTO>(skill));
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SkillDTO dto, CancellationToken cancellationToken = default)
        {
            var skill = _mapper.Map<Skill>(dto);

            _skillRepository.Add(skill);
            await _skillRepository.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(nameof(Get), new { skill.Id }, _mapper.Map<SkillDTO>(skill));
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SkillDTO dto, CancellationToken cancellationToken)
        {
            var skill = await _skillRepository.FindByIdAsync(dto.Id);
            if (skill is null)
                return NotFound();

            _mapper.Map(dto, skill);
            _skillRepository.Update(skill);
            await _skillRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var skill = await _skillRepository.FindByIdAsync(id);
            if (skill is null)
                return NotFound("No Skill Found");

            _skillRepository.Delete(skill);
            await _skillRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}
