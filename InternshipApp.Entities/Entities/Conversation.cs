using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipApp.Core.Entities;

public class Conversation : BaseEntity<int>
{
    public string? Title { get; set; }

    public string? Pin { get; set; }


    public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
    public virtual ICollection<Message> Messages { get; set; } = new HashSet<Message>();
}
