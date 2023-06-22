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

    public Task<List<InternSettings>> FindBySkillAsync(int settingId)
        => FindAll(x => x.Id == settingId).ToListAsync();
}
