using InternshipApp.Contracts; using InternshipApp.Core.Database; using InternshipApp.Core.Entities;

namespace InternshipApp.Repository
{

	public class MessageRepository : BaseRepository<Message>, IMessageRepository
	{
		public MessageRepository(AppDbContext context) : base(context) { }

        
	}
}