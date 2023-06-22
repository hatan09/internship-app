﻿using InternshipApp.Core.Entities;

namespace InternshipApp.Contracts;

public interface IInternSettingsRepository : IBaseRepository<InternSettings>
{
    public Task<List<InternSettings>> FindBySkillAsync(int settingId);
}
