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
    public class InternGroupController : ControllerBase
    {
        private readonly IInternGroupRepository _internGroupRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IConversationRepository _conversationRepository;
        private readonly InstructorManager _instructorManager;
        private readonly StudentManager _studentManager;
        private readonly IMapper _mapper;

        public InternGroupController(
            IDepartmentRepository departmentRepository, 
            InstructorManager instructorManager, StudentManager studentManager, 
            IInternGroupRepository internGroupRepository,
            IConversationRepository conversationRepository,
            IMapper mapper)
        {
            _instructorManager = instructorManager;
            _departmentRepository = departmentRepository;
            _internGroupRepository = internGroupRepository;
            _conversationRepository = conversationRepository;
            _studentManager = studentManager;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var internGroups = await _internGroupRepository.FindAll().Include(grp => grp.Department).Include(grp => grp.Instructor).ToListAsync(cancellationToken);
            if (internGroups is null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<InternGroupDTO>>(internGroups));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var internGroup = await _internGroupRepository.FindByIdAsync(id);
            if (internGroup is null)
                return NotFound();

            return Ok(_mapper.Map<InternGroupDTO>(internGroup));
        }


        [HttpGet("{instructorId}")]
        public async Task<IActionResult> GetByInstructor(string instructorId, CancellationToken cancellationToken = default)
        {
            var instructor = await _instructorManager.FindByIdAsync(instructorId);
            if (instructor is null)
                return NotFound("No Instructor Found");

            var group = await _internGroupRepository.FindAll(grp => grp.InstructorId!.Equals(instructorId)).FirstOrDefaultAsync(cancellationToken);

            return Ok(_mapper.Map<InternGroupDTO>(group));
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InternGroupDTO dto, CancellationToken cancellationToken = default)
        {
            var internGroup = _mapper.Map<InternGroup>(dto);

            var department = await _departmentRepository.FindByIdAsync(dto.DepartmentId, cancellationToken);
            if(department is null) return NotFound("No Department Found");

            var instructor = await _instructorManager.FindByIdAsync(dto.InstructorId);
            if (instructor is null) return NotFound("No Instructor Found");

            internGroup.Department = department;
            internGroup.Instructor = instructor;

            _internGroupRepository.Add(internGroup);
            await _internGroupRepository.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(nameof(Get), new { internGroup.Id }, _mapper.Map<InternGroupDTO>(internGroup));
        }


        [HttpPut("{groupId}")]
        public async Task<IActionResult> AddStudents([FromBody] List<string> studentIds, int groupId, CancellationToken cancellationToken)
        {
            if(studentIds == null || studentIds.Count <= 0)
            {
                return BadRequest("No student provided!");
            }

            var internGroup = await _internGroupRepository.FindAll(x => x.Id == groupId).Include(x => x.Students).Include(x => x.Instructor).FirstOrDefaultAsync(cancellationToken);
            if (internGroup is null || internGroup.Instructor == null)
                return NotFound();

            var students = await _studentManager.FindAll(x => studentIds.Contains(x.Id)).ToListAsync(cancellationToken);
            students.ForEach(x =>
            {
                if(x != null)
                    internGroup.Students.Add(x);
            });

            _internGroupRepository.Update(internGroup);
            await _internGroupRepository.SaveChangesAsync(cancellationToken);

            students.ForEach(student => {
                var newConversation = new Conversation()
                {
                    LastMessageTime = DateTime.Now,
                    Users = { student, internGroup.Instructor },
                    Title = $"{internGroup.Instructor.FullName}_{student.FullName}"
                };
                _conversationRepository.Add(newConversation);
            });

            await _conversationRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] InternGroupDTO dto, CancellationToken cancellationToken)
        {
            var internGroup = await _internGroupRepository.FindByIdAsync(dto.Id);
            if (internGroup is null)
                return NotFound();

            _mapper.Map(dto, internGroup);
            _internGroupRepository.Update(internGroup);
            await _internGroupRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var internGroup = await _internGroupRepository.FindByIdAsync(id, cancellationToken);
            if (internGroup is null)
                return NotFound("No Group Found");

            _internGroupRepository.Delete(internGroup);
            await _internGroupRepository.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}
