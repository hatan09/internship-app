using AutoMapper;
using InternshipApp.Api.DataObjects;
using InternshipApp.Contracts;
using InternshipApp.Core.Entities;
using InternshipApp.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternshipApp.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EvaluationController : ControllerBase
    {
        private readonly IEvaluationRepository _evaluationRepository;
        private readonly StudentManager _studentManager;
        private readonly IMapper _mapper;

        public EvaluationController(
            StudentManager studentManager,
            IEvaluationRepository evaluationRepository,
            IMapper mapper)
        {
            _studentManager = studentManager;
            _evaluationRepository = evaluationRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var evaluations = await _evaluationRepository.FindAll().ToListAsync(cancellationToken);
            if (evaluations is null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<EvaluationDTO>>(evaluations));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var evaluation = await _evaluationRepository.FindByIdAsync(id);
            if (evaluation is null)
                return NotFound();

            return Ok(_mapper.Map<EvaluationDTO>(evaluation));
        }


        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetByStudent(string studentId, CancellationToken cancellationToken = default)
        {
            var student = await _studentManager.FindByIdAsync(studentId);
            if (student is null)
                return NotFound("No Student Found");

            var group = await _evaluationRepository.FindAll(x => x.StudentId!.Equals(studentId)).FirstOrDefaultAsync(cancellationToken);

            return Ok(_mapper.Map<EvaluationDTO>(group));
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EvaluationDTO dto, CancellationToken cancellationToken = default)
        {
            var student = await _studentManager.FindByIdAsync(dto.StudentId);
            if (student is null) return NotFound("No Student Found");

            var evaluation = _mapper.Map<Evaluation>(dto);

            _evaluationRepository.Add(evaluation);
            await _evaluationRepository.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(nameof(Get), new { evaluation.Id }, _mapper.Map<EvaluationDTO>(evaluation));
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] EvaluationDTO dto, CancellationToken cancellationToken)
        {
            var evaluation = await _evaluationRepository.FindByIdAsync(dto.Id, cancellationToken);
            if (evaluation is null)
                return NotFound();

            _mapper.Map(dto, evaluation);
            _evaluationRepository.Update(evaluation);
            await _evaluationRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var evaluation = await _evaluationRepository.FindByIdAsync(id, cancellationToken);
            if (evaluation is null)
                return NotFound("No Group Found");

            _evaluationRepository.Delete(evaluation);
            await _evaluationRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}
