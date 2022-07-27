using Player;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponFatory : WeaponFactory
{
    private Dictionary<int, MeleeWeaponData> weapons;

    public MeleeWeaponFatory()
    {
        LoadData("MeleeWeaponData");
        weapons = new Dictionary<int, MeleeWeaponData>();
    }

    public override WeaponData CreateWeapon(int id)
    {
        if (id == 0)
        {
            Debug.LogError("没有ID为0的武器，CSV第一排为字段名");
        }

        //返回拷贝出来的一份实例
        MeleeWeaponData wp = null;
        if (weapons.TryGetValue(id,out wp))
        {
            return new MeleeWeaponData(wp);
        }

        wp = new MeleeWeaponData();
        string[] row = allData[id].Split(new char[] { ',' });

        wp.weaponName = row[1];
        int.TryParse(row[2], out wp.damage);
        float.TryParse(row[3], out wp.atkAngle);
        wp.prefab = Resources.Load<GameObject>(wp.weaponName);
        weapons.Add(id, wp);

        return new MeleeWeaponData(wp);
    }
}

