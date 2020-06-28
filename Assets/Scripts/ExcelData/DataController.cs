using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using GameCore;
using System;

public class DataController :Singleton<DataController> {
    //引用GameCore命名空间，继承Singleton试用单例模式
    //数据控制类，加载读取配置表

    //用于存放背包配置表的字典<字段名<ID，字段值>>
    public Dictionary<string, Dictionary<string, string>> dicPack;
    //用于存放公告配置表的字典<字段名<ID，字段值>>
    public Dictionary<string, Dictionary<string, string>> dicNotice;
    //用于存放关卡配置表的字典<字段名<ID，字段值>>
    public Dictionary<string, Dictionary<string, string>> dicLevel;
    //用于存放角色配置表的字典<字段名<ID，字段值>>
    public Dictionary<string, Dictionary<string, string>> dicPlayertshe;

    //加载全部配置表
    public void LoadAllCfg()
    {
        //LoadPackCfg();//背包
        //LoadLevelCfg();//关卡
        //LoadPlayerCfg();//角色
        //LoadNoticeCfg();//公告
    }

    private void LoadNoticeCfg()//加载公告配置表
    {
        ExcelData.LoadExcelFormCSV("NoticeCfg", out dicNotice);
    }

    private void LoadPackCfg()//加载背包配置表
    {
        ExcelData.LoadExcelFormCSV("PackCfg",out dicPack);
    }
    
    private void LoadLevelCfg()//加载关卡配置表
    {
        ExcelData.LoadExcelFormCSV("LevelCfg", out dicLevel);
    }
    
    private void LoadPlayerCfg()//加载角色配置表
    {
        ExcelData.LoadExcelFormCSV("PlayertsheCfg", out dicPlayertshe);
    }
    
    public string ReadCfg(string keyName, int id, Dictionary<string, Dictionary<string, string>> dic)//供外界调用的读取配置表字段值
    {
        return dic[keyName][id.ToString()];
    }
    //对外提供的，读取配置表数据量的方法（几行真实数据，要排除前3行）
    public int GetCfgCount(Dictionary<string, Dictionary<string, string>> dic)
    {
        int count = 0;
        foreach (KeyValuePair<string, Dictionary<string, string>> item in dic)
        {
            count = item.Value.Count;
            break;
        }
        return count;
    }

}
