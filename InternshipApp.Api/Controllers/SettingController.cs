using AutoMapper;
using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternshipApp.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InternSettingsController : ControllerBase
    {
        private readonly IInternSettingsRepository _internSettingsRepository;
        private readonly IMapper _mapper;

        public InternSettingsController(IInternSettingsRepository internSettingsRepository, IMapper mapper)
        {
            _internSettingsRepository = internSettingsRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var internSettings = await _internSettingsRepository.FindAll().ToListAsync(cancellationToken);

            return Ok(internSettings);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var internSettings = await _internSettingsRepository.FindByIdAsync(id);
            if (internSettings is null)
                return NotFound();

            return Ok(internSettings);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InternSettings internSettings, CancellationToken cancellationToken = default)
        {
            _internSettingsRepository.Add(internSettings);
            await _internSettingsRepository.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(nameof(Get), new { internSettings.Id }, internSettings);
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] InternSettings settings, CancellationToken cancellationToken)
        {
            var internSettings = await _internSettingsRepository.FindByIdAsync(settings.Id, cancellationToken);
            if (internSettings is null)
                return NotFound();

            _internSettingsRepository.Update(settings);
            await _internSettingsRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var internSettings = await _internSettingsRepository.FindByIdAsync(id, cancellationToken);
            if (internSettings is null)
                return NotFound("No InternSettings Found");

            _internSettingsRepository.Delete(internSettings);
            await _internSettingsRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}
