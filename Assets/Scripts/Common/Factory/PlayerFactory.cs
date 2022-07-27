using Player;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFactory : CharacterFactory
{
    protected Dictionary<int, PlayerData> players; 

    public PlayerFactory()
    {
        LoadData("Player");
        players = new Dictionary<int, PlayerData>();
    }

    public override CharacterData Create(int id)
    {
        PlayerData data = null;
        if (players.TryGetValue(id,out data))
        {
            return new PlayerData(data);
        }

        data = new PlayerData();
        string[] row = allData[id].Split(new char[] { ',' });

        int.TryParse(row[2], out data.maxHp);
        float.TryParse(row[3], out data.moveSpeed);

        players.Add(id, data);
        return new PlayerData(data);
    }
}
