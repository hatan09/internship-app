using InternshipApp.Contracts;
using InternshipApp.Core.Database;
using InternshipApp.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternshipApp.Repository;

public class SkillScoreRepository : BaseRepository<SkillScore>, ISkillScoreRepository
{
    public SkillScoreRepository(AppDbContext context) : base(context)
    {
    }

    public Task<List<SkillScore>> FindBySkillAsync(int skillId)
        => FindAll(x => x.SkillId == skillId).ToListAsync();

        
}
