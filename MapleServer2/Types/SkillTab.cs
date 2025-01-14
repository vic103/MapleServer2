﻿using Maple2Storage.Types.Metadata;
using MapleServer2.Constants.Skills;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;

namespace MapleServer2.Types;

public class SkillTab
{
    public long Uid { get; private set; }
    public long TabId { get; set; }
    public string Name { get; set; }

    public List<int> Order { get; private set; }
    public Dictionary<int, SkillMetadata> SkillJob { get; private set; }
    public Dictionary<int, short> SkillLevels { get; private set; }

    public SkillTab() { }

    public SkillTab(long characterId, Job job, long id, string name)
    {
        Name = name;
        ResetSkillTree(job);
        TabId = id;
        Uid = DatabaseManager.SkillTabs.Insert(this, characterId);
    }

    public SkillTab(string name, int jobId, long tabId, long uid, Dictionary<int, short> skillLevels)
    {
        Name = name;
        TabId = tabId;
        Uid = uid;
        GenerateSkills((Job) jobId);
        SkillLevels = skillLevels;
    }

    public static Dictionary<int, SkillMetadata> AddOnDictionary(Job job)
    {
        Dictionary<int, SkillMetadata> skillJob = new();

        foreach (SkillMetadata skill in SkillMetadataStorage.GetJobSkills(job))
        {
            skillJob[skill.SkillId] = skill;
        }
        return skillJob;
    }

    public void AddOrUpdate(int id, short level, bool isLearned)
    {
        SkillLevels[id] = isLearned ? level : (short) 0;
        if (!SkillJob.ContainsKey(id))
        {
            return;
        }

        foreach (int sub in SkillJob[id].SubSkills)
        {
            SkillLevels[sub] = isLearned ? level : (short) 0;
        }
    }

    public void GenerateSkills(Job job)
    {
        Order = SkillTreeOrdered.GetListOrdered(job);
        SkillJob = AddOnDictionary(job);
    }

    public void ResetSkillTree(Job job)
    {
        GenerateSkills(job);
        SkillLevels = SkillJob.ToDictionary(x => x.Key, x => x.Value.CurrentLevel);
    }

    public static List<SkillMetadata> GetJobFeatureSkills(Job job)
    {
        return SkillMetadataStorage.GetJobSkills(job);
    }

    public override string ToString()
    {
        return $"SkillTab(Id:{Uid},Name:{Name},Skills:{string.Join(",", SkillJob)})";
    }
}
