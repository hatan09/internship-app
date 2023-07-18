using InternshipApp.Core.Entities;

namespace InternshipApp.Api.DataObjects
{
    public class CompanyDTO : BaseDTO<int>
    {
        public string Title { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? CompanyWebsite { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImgUrl { get; set; } = string.Empty;
        public CompanyType Type { get; set; } = CompanyType.OTHER;
    }
}
