﻿using MapleServer2.Types;

namespace MapleServer2.Managers;

public class ClubManager
{
    private readonly Dictionary<long, Club> ClubList;

    public ClubManager()
    {
        ClubList = new();
    }

    public void AddClub(Club club)
    {
        ClubList.Add(club.Id, club);
    }

    public void RemoveClub(Club club)
    {
        ClubList.Remove(club.Id);
    }

    public Club GetClubById(long id)
    {
        return ClubList.TryGetValue(id, out Club foundClub) ? foundClub : null;
    }

    public Club GetClubByLeader(Player leader)
    {
        return (from entry in ClubList
                where entry.Value.Leader.CharacterId == leader.CharacterId
                select entry.Value).FirstOrDefault();
    }
}
