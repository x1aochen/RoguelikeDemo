using System.Collections.Generic;
using UnityEngine;
using Player;
using System;
using System.Reflection;



public static class LoadGameData//:MonoSingleton<LoadGameData>
{
    private static WeaponFactory meleeFactory;
    private static WeaponFactory remoteFactory;
    private static CharacterFactory enemyFactory;
    private static CharacterFactory playerFactory;
    private static Dictionary<string, AIConfigurationReader> cache;

    static LoadGameData()
    {
        meleeFactory = new MeleeWeaponFatory();
        remoteFactory = new RemoteWeaponFactory();
        enemyFactory = new EnemyFactory();
        playerFactory = new PlayerFactory();
        cache = new Dictionary<string, AIConfigurationReader>();
    }


    /// <summary>
    /// 得到初始的武器
    /// </summary>
    /// <returns></returns>
    public static WeaponData[] GetInitWeapon()
    {
        //TODO:根据存储的数据得到上一次退出游戏时所持武器
        WeaponData[] wps = new WeaponData[2];
        wps[0] = remoteFactory.CreateWeapon(3);
        wps[1] = meleeFactory.CreateWeapon(1);
        return wps;
    }

    public static WeaponData GetWeaponData(int id)
    {
        WeaponType type;
        if (id < 100)
        {
            type = WeaponType.Remote;
        }
        else
        {
            type = WeaponType.Melee;
        }

        switch (type)
        {
            case WeaponType.Remote:
                return remoteFactory.CreateWeapon(id);
            case WeaponType.Melee:
                return meleeFactory.CreateWeapon(id - 100);
        }
        return null;
    }

    public static CharacterData GetEnemyData(int id)
    {
        return enemyFactory.Create(id);
    }
    
    /// <summary>
    /// 得到AI对应状态表
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static Dictionary<string,Dictionary<string,string>> LoadAIMap(string fileName)
    {
        AIConfigurationReader map;
        if (cache.TryGetValue(fileName,out map))
        {
            return map.map;
        }

        map = new AIConfigurationReader(fileName);
        cache.Add(fileName, map);
        return map.map;
    }

    public static CharacterData GetPlayerData(int id)
    {
        return playerFactory.Create(id);

    }

}
