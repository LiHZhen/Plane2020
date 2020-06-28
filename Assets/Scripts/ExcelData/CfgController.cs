using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CfgController : Singleton<CfgController> {

    //储存敌人配置表的字典
    public Dictionary<string, EnemyCfg> dicEnemy;
    //用于存放关卡配置表的字典
    public Dictionary<string, LevelCfg> dicLevel;
    //用于存放物品配置表的字典
    public Dictionary<string, ItemCfg> dicItem;
    //用于存放飞机配置表的字典
    public Dictionary<string, PlaneCfg> dicPlane;


    //加载所有配置表
    public void LoadAllCfg()
    {
        LoadCfgFromJson("ItemCfg", out dicItem);
        LoadCfgFromJson("LevelCfg", out dicLevel);
        LoadCfgFromJson("EnemyCfg", out dicEnemy);
        LoadCfgFromJson("PlaneCfg", out dicPlane);
    }

    /// <summary>
    /// 外界调用物品配置表方法
    /// </summary>
    /// <param name="id">物品ID</param>
    /// <returns>物品类</returns>
    public ItemCfg ReadItemCfg(string id)
    {
        if (dicItem.ContainsKey(id))
        {
            return dicItem[id];
        }

        return null;
    }

    /// <summary>
    /// 外界调用关卡配置表方法
    /// </summary>
    /// <param name="id">关卡ID</param>
    /// <returns>单个关卡</returns>
    public LevelCfg ReadLevelCfg(string id)
    {
        if (dicLevel.ContainsKey(id))
        {
            return dicLevel[id];
        }

        return null;
    }

    /// <summary>
    /// 外界调用敌人配置表方法
    /// </summary>
    /// <param name="id">敌人ID</param>
    /// <returns>单个关卡</returns>
    public EnemyCfg ReadEnemyCfg(string id)
    {
        if (dicEnemy.ContainsKey(id))
        {
            return dicEnemy[id];
        }

        return null;
    }

    /// <summary>
    /// 外界调用飞机配置表方法
    /// </summary>
    /// <param name="id">飞机等级</param>
    /// <returns>单个关卡</returns>
    public PlaneCfg ReadPlaneCfg(string id)
    {
        if (dicPlane.ContainsKey(id))
        {
            return dicPlane[id];
        }

        return null;
    }



    private void LoadCfgFromJson<T,U>(string name, out Dictionary<T, U> dicCfg)
    {
        TextAsset data=Resources.Load("JsonCfg/"+name)as TextAsset;
        string jsonStr = data.text;
        dicCfg = JsonMapper.ToObject<Dictionary<T,U>>(jsonStr);
    }
}
