using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AIConfigurationReader
{
    //private Dictionary<string, Dictionary<string, string>> map;
    public Dictionary<string, Dictionary<string, string>> map { get; set; }

    public AIConfigurationReader(string fileName)
    {
        map = new Dictionary<string, Dictionary<string, string>>();
        LoadAIMap(fileName);
    }

    private void LoadAIMap(string fileName)
    {
        string mainkey = "";
        TextAsset content = Resources.Load<TextAsset>(Consts.CSVPath + fileName);

        string[] data = content.text.Split(new char[] { '\n' });
        map = new Dictionary<string, Dictionary<string, string>>();

        for (int i = 0; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });

            //主键 状态详情
            if (row[0].StartsWith(">"))
            {
                mainkey = row[0].Substring(1, row[0].Length - 1);
                map.Add(mainkey, new Dictionary<string, string>());
            }
            //状态与条件映射
            else
            {
                map[mainkey].Add(row[0], row[1]);
            }
        }
    }

}

