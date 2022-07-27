using UnityEngine;
using Player;
using System.Collections.Generic;

public class RemoteWeaponFactory : WeaponFactory
{
    protected Dictionary<int, RemoteWeaponData> weapons;
    public RemoteWeaponFactory()
    {
        LoadData("RemoteWeaponData");
        weapons = new Dictionary<int, RemoteWeaponData>();
    }

    public override WeaponData CreateWeapon(int id)
    {
        if (id == 0)
        {
            Debug.LogError("û��IDΪ0��������CSV��һ��Ϊ�ֶ���");
        }

        //���ؿ���������һ��ʵ��
        RemoteWeaponData wp = null;
        if (weapons.TryGetValue(id,out wp))
        {
            return new RemoteWeaponData(wp);
        }

        wp = new RemoteWeaponData();
        string[] row = allData[id].Split(new char[] { ',' });

        wp.weaponName = row[1];
        int.TryParse(row[2], out wp.damage);
        float.TryParse(row[3], out wp.atkInterval);
        int.TryParse(row[4], out wp.bulletAmount);
        int.TryParse(row[5], out wp.magazineCapacity);
        wp.bulletName = row[6];
        wp.fireEffectName = row[7];
        float.TryParse(row[8], out wp.bulletSpeed);
        float.TryParse(row[9], out wp.fillTime);
        float.TryParse(row[10], out wp.attackRange);
        wp.prefab = Resources.Load<GameObject>(wp.weaponName);
        wp.bullet = Resources.Load<GameObject>(wp.bulletName);
        wp.fireEffect = Resources.Load<GameObject>(wp.fireEffectName);
        weapons.Add(id, wp);

        return new RemoteWeaponData(wp);
    }
}
