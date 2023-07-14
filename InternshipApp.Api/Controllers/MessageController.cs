using AutoMapper; using Microsoft.AspNetCore.Mvc; using Microsoft.EntityFrameworkCore; using InternshipApp.Core.Database; using InternshipApp.Core.Entities; using InternshipApp.Api.DataObjects;
using InternshipApp.Contracts;

namespace InternshipApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;
        private readonly IMessageRepository _messageRepository;
					
        public MessageController(	IMapper mapper,
                                    AppDbContext appDbContext,
							        IMessageRepository messageRepository) {
	        _mapper = mapper;
	        _appDbContext = appDbContext;
	        _messageRepository = messageRepository;
						
        }
					
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
	        var messages = await _messageRepository.FindAll().ToListAsync(cancellationToken);
	        return Ok(_mapper.Map<IEnumerable<MessageDTO>>(messages));
        }


        [HttpGet("{messageId}")]
        public async Task<IActionResult> GetById(int messageId, CancellationToken cancellationToken = default)
        {
	        var message = await _messageRepository.FindByIdAsync(messageId, cancellationToken);
	        return message != null ? Ok(_mapper.Map<MessageDTO>(message)) : NotFound("Unable to find the requested message"); 
        }
					
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MessageDTO dto, CancellationToken cancellationToken = default)
        {
	        using var appTransaction = await _appDbContext.Database.BeginTransactionAsync();
	        var message = _mapper.Map<Message>(dto);
	        if (message != null)
	        {
		        _messageRepository.Add(message);
		        await appTransaction.CommitAsync(cancellationToken);
		        return Ok(message.Id);
	        }
	        else return BadRequest("Can't convert request to Message");
        }
					
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] MessageDTO dto, int id, CancellationToken cancellationToken = default)
        {
	        var Message = await _messageRepository.FindByIdAsync(id, cancellationToken);
	        if (Message is null)
		        return NotFound();

	        _mapper.Map(dto, Message);
	        await _messageRepository.SaveChangesAsync(cancellationToken);
	        return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
	        var message = await _messageRepository.FindByIdAsync(id, cancellationToken);
	        if (message is null)
		        return NotFound();

	        _messageRepository.Delete(message);
						

	        await _messageRepository.SaveChangesAsync(cancellationToken);
	        return NoContent();
        }
    }
}