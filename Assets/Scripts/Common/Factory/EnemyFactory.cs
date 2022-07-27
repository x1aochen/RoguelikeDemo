using UnityEngine;
using Player;
using System.Collections.Generic;
using System;
using AI;
using System.Reflection;

public class EnemyFactory:CharacterFactory
{
    protected Dictionary<int, EnemyData> enemies;

    public EnemyFactory()
    {
        LoadData("AIEnemy");
        enemies = new Dictionary<int, EnemyData>();
    }


    public override CharacterData Create(int id)
    {
        EnemyData data = null;
        if (enemies.TryGetValue(id,out data))
        {
            return new EnemyData(data);
        }

        data = new EnemyData();
        string[] row = allData[id].Split(new char[] { ',' });

        Enum.TryParse(row[0], out data.type);   
        int.TryParse(row[1], out data.maxHp);
        float.TryParse(row[2], out data.moveSpeed);
        float.TryParse(row[3], out data.runSpeed);
        float.TryParse(row[4], out data.sightDist);
        float.TryParse(row[5], out data.attackDist);
        int.TryParse(row[6], out data.damage);
        data.configurationFile = row[7];
        int.TryParse(row[8], out data.sightAngle);
        float.TryParse(row[9], out data.detectionInterval);
        float.TryParse(row[10], out data.atkInterval);
        int.TryParse(row[11], out data.wpID);

        data.prefab = Resources.Load<GameObject>(Consts.enemyPath + data.type);
       
        enemies.Add(id, data);
        return new EnemyData(data);
    }
}
