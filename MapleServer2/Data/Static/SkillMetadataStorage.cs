﻿using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants.Skills;
using MapleServer2.Enums;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class SkillMetadataStorage
{
    private static readonly Dictionary<int, SkillMetadata> skill = new();

    public static void Init()
    {
        using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-skill-metadata");
        List<SkillMetadata> skillList = Serializer.Deserialize<List<SkillMetadata>>(stream);
        foreach (SkillMetadata skills in skillList)
        {
            skill[skills.SkillId] = skills;
        }
    }

    public static SkillMetadata GetSkill(int id) => skill.GetValueOrDefault(id);

    public static List<int> GetEmotes() => skill.Values.Where(x => x.SkillId / 100000 == 902).Select(x => x.SkillId).ToList();

    // Get a List of Skills corresponding to the Job
    public static List<SkillMetadata> GetJobSkills(Job job = Job.None)
    {
        List<SkillMetadata> jobSkill = new();
        List<int> gmSkills = SkillTreeOrdered.GetListOrdered(Job.GameMaster);

        if (Job.GameMaster == job)
        {
            foreach (int skillId in gmSkills)
            {
                jobSkill.Add(skill[skillId]);
                jobSkill.First(skill => skill.SkillId == skillId).CurrentLevel = 1;
            }
            return jobSkill;
        }

        foreach (KeyValuePair<int, SkillMetadata> skills in skill)
        {
            if (skills.Value.Job == (int) job)
            {
                jobSkill.Add(skills.Value);
            }
            else if (skills.Value.SkillId == 20000001) // Swiming
            {
                jobSkill.Add(skills.Value);
                skills.Value.CurrentLevel = 1;
            }
            else if (skills.Value.SkillId == 20000011) // Climbing walls
            {
                jobSkill.Add(skills.Value);
                skills.Value.CurrentLevel = 1;
            }
        }
        return jobSkill;
    }
}
