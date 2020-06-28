using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System;
using UiCore;

public class SweepData : Model {

    //保存关卡扫荡次数字典<关卡id,扫荡次数>
    private Dictionary<string, int> dicSweep=new Dictionary<string, int>();

    //private string path=Application.persistentDataPath + "\\SweepData.json";//保存数据的路径

    public override string Name
    {
        get { return "SweepData"; }
    }

    public override string Path
    {
        get
        {
            return Application.persistentDataPath + "\\SweepData.json";
        }
    }

    protected override void JsonToObj()
    {
        dicSweep = JsonMapper.ToObject<Dictionary<string,int>>(json);
    }

    public int ReadCountByKey(string key)
    {
        if (dicSweep.ContainsKey(key))
        {
            return dicSweep[key];
        }
        return -1;
    }

    public void AddCountByKey(string key, int value)
    {
        dicSweep[key] += value;
        SaveData(dicSweep);
    }

    public void EditCountByKey(string key, int value)
    {
        dicSweep[key] = value;
        SaveData(dicSweep);
    }

    /// <summary>
    /// 不是当天，重置次数
    /// </summary>
    public void ResetValue()
    {
        dicSweep.Clear();
        SaveData(dicSweep);
    }
}
