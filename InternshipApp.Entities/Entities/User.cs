﻿using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace InternshipApp.Core.Entities
{
    public class User : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public bool? Gender { get; set; }
        public DateTime Birthdate { get; set; }
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<UserRole> UserRoles { get; } = new HashSet<UserRole>();

        public virtual ICollection<Conversation> Conversations { get; set; } = new HashSet<Conversation>();

        [JsonIgnore]
        public virtual ICollection<Message> Messages { get; set; } = new HashSet<Message>();
    }
}