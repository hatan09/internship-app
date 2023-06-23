using InternshipApp.Contracts;
using InternshipApp.Core.Database;
using InternshipApp.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipApp.Repository;

public class InternSettingsRepository : BaseRepository<InternSettings>, IInternSettingsRepository
{
    public InternSettingsRepository(AppDbContext context) : base(context)
    {
    }

    public Task<InternSettings?> GetCurrentSemester()
        => FindAll(x => x.StartTime <= DateTime.Today && x.EndTime >= DateTime.Today).FirstOrDefaultAsync();
}
