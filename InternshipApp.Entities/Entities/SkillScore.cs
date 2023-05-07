using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipApp.Core.Entities;

public class SkillScore : BaseEntity<int>
{
    public int SkillId { get; set; }

    public int AlternativeSkillId { get; set; }

    public MatchingType Matching { get; set; }

}

public enum MatchingType { FIT, NEARLYFIT, AVERAGE }
