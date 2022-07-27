using UnityEngine;
using Player;
public abstract class CharacterFactory
{
    protected string[] allData;
    protected void LoadData(string fileName)
    {
        allData = Resources.Load<TextAsset>(Consts.CSVPath + fileName).text.Split(new char[] { '\n' });
    }
    public abstract CharacterData Create(int id);

}
