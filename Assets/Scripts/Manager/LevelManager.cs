using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelModel
{
    //最大关卡数
    public int currentMaxLevel = 1;
    //关卡ID，星数
    public Dictionary<string , int> dicLevelStar=new Dictionary<string, int>();
}

public class LevelManager : UnitySingleton<LevelManager>
{
    //保存数据的路径
    private string path;
    private int currentMaxLevel = 1;
    //当前进入关卡
    private int currentEnterLevel = 0;

    LevelModel levelModel;
    // Use this for initialization
    void Awake () {
        path = Application.persistentDataPath + "\\LevelManager.json";
        //Debug.Log(path);
        levelModel = new LevelModel();
        if (!File.Exists(path))
        {
            //levelModel.dicLevelStar.Add("1",0);
            SaveData();
            currentMaxLevel = 1;
        }
        else
        {
            CoroutineController.Instance.StartCoroutine(ReadData());
        }


        //if (!GameTool.HasKey("CurrentLevel"))
        //{
        //    GameTool.SetInt("CurrentLevel",1);
        //    currentMaxLevel = 1;
        //}
        //else
        //{
        //    currentMaxLevel = GameTool.GetInt("CurrentLevel");
        //}
        //检查当前关卡
    }

    public void SetCurrentEnterLevel(int current)
    {
        currentEnterLevel = current;
    }

    public int GetCurrentEnterLevel()
    {
        return currentEnterLevel;
    }

    public int GetCurrentMaxLevel()
    {
        return currentMaxLevel;
    }
    public void SetCurrentMaxLevel(int current)
    {
            currentMaxLevel = current;
            levelModel.currentMaxLevel=current;
            SaveData();
    }

    /// <summary>
    /// 保存最高关卡
    /// </summary>
    void SaveData()
    {
        string jsonData = JsonMapper.ToJson(levelModel);
        using (StreamWriter sw = new StreamWriter(path))
        {
            sw.WriteLine(jsonData);
        }
    }

    /// <summary>
    /// 更新储存内的最高光卡
    /// </summary>
    /// <returns></returns>
    IEnumerator ReadData()
    {
        //Debug.Log(path);
        WWW www = new WWW("file:///" + path);
        yield return www;
        string json = www.text;

        levelModel = JsonMapper.ToObject<LevelModel>(json);
        currentMaxLevel = levelModel.currentMaxLevel;
    }

    //修改星级评价的方法
    public void ChangeLevelStar(int id, int starCount)
    {
        if (levelModel.dicLevelStar.ContainsKey(id.ToString()))
        {
            levelModel.dicLevelStar[id.ToString()] = starCount;

        }
        else
        {
            levelModel.dicLevelStar.Add(id.ToString(), starCount);
        }
        SaveData();
    }

    //根据关卡ID获取该关卡的星数
    public int GetStarCount(int id)
    {
        if (levelModel.dicLevelStar.ContainsKey(id.ToString()))
        {
            return levelModel.dicLevelStar[id.ToString()];
        }
        return 0;
    }


    // Update is called once per frame
    void Update () {
		
	}
}
