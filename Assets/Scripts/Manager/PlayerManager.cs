using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Player
{
    public int planeRank;
}

public class PlayerManager : UnitySingleton<PlayerManager>
{
    Player player=new Player();

    private string path;


    public int PlaneRank
    {
        get
        {
            return player.planeRank;
        }

        set
        {
            //Debug.Log(value);
            player.planeRank = value;
            SaveData();
        }
    }

    void Awake()
    {
        InitPlayer();
    }

    
    void Start ()
    {

    }

    /// <summary>
    /// 读取玩家、飞机等级
    /// </summary>
    private void InitPlayer()
    {
        path = Application.persistentDataPath + "\\Player.json";
        Debug.Log(path);
        if (!File.Exists(path))
        {
            player.planeRank = 1;
            SaveData();
        }
        else
        {
            CoroutineController.Instance.StartCoroutine(ReadData());
        }
    }
    private void SaveData()
    {
        //Debug.Log(PlaneRank);
        string jsonData = JsonMapper.ToJson(player);
        using (StreamWriter sw = new StreamWriter(path))
        {
            sw.WriteLine(jsonData);
        }
    }
    IEnumerator ReadData()
    {
        //Debug.Log(PlaneRank);
        WWW www = new WWW("file:///" + path);
        yield return www;
        string json = www.text;
        player = JsonMapper.ToObject<Player>(json);
        //Debug.Log(player.planeRank);
    }
}
