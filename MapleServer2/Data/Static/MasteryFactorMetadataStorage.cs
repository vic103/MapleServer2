﻿using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class MasteryFactorMetadataStorage
{
    private static readonly Dictionary<int, MasteryFactorMetadata> map = new();

    public static void Init()
    {
        using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-mastery-factor-metadata");
        List<MasteryFactorMetadata> masteryFactors = Serializer.Deserialize<List<MasteryFactorMetadata>>(stream);
        foreach (MasteryFactorMetadata masteryFactor in masteryFactors)
        {
            map[masteryFactor.Differential] = masteryFactor;
        }
    }

    public static List<int> GetMasteryFactorIds()
    {
        return new(map.Keys);
    }

    public static int GetFactor(int id)
    {
        return map.GetValueOrDefault(id).Factor;
    }
}
