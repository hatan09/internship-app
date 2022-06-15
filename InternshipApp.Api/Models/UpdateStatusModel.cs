using InternshipApp.Core.Entities;

namespace InternshipApp.Api.Models
{
    public class UpdateStatusModel
    {
        public string Id { get; set; } = string.Empty;

        public Stat Stat { get; set; } = Stat.PENDING;
    }
}
