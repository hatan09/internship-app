using System.ComponentModel.DataAnnotations;

namespace InternshipApp.Api.DataObjects
{
    public class RoleDTO : BaseDTO<string>
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
