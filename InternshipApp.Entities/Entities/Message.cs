using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipApp.Core.Entities;

public class Message : BaseEntity<int>
{
    public string Content { get; set; }

    public DateTime SentTime { get; set; }

    public int ConversationId { get; set; }
    public Conversation Conversation { get; set; }

    public string UserId { get; set; } = string.Empty;
    public User User { get; set; } = default!;
}
