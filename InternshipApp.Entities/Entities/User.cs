using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace InternshipApp.Core.Entities
{
    public class User : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public bool? Gender { get; set; }
        public DateTime Birthdate { get; set; }
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<UserRole> UserRoles { get; } = new HashSet<UserRole>();
    }
}
