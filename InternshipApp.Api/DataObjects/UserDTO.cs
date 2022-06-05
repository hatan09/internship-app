using System.ComponentModel.DataAnnotations;

namespace InternshipApp.Api.DataObjects
{
    public class UserDTO : BaseDTO<string>
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Birthdate { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public ICollection<string> Roles { get; set; } = Array.Empty<string>();
    }
}
