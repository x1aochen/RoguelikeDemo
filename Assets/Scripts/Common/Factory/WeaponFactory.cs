using Player;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponFactory
{

    protected string[] allData;

    protected void LoadData(string fileName)
    {
        allData = Resources.Load<TextAsset>(Consts.CSVPath + fileName).text.Split(new char[] { '\n' });
    }


    public abstract WeaponData CreateWeapon(int id);
}

