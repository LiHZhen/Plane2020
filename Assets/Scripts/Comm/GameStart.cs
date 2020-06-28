using System.Collections;
using System.Collections.Generic;
using UiCore;
using UnityEngine;

public class GameStart : MonoBehaviour
{

    private GameObject mainCanvas=null;
    
    void Awake () {
        mainCanvas=GameObject.Find("MainCanvas(Clone)");
        if (mainCanvas==null)
        {
            Instantiate(Resources.Load("UIPrefabs/MainCanvas"));
            
        }
        //PlayerData.Instance.InitPlayerData();
        //DataController.Instance.LoadAllCfg();
        //MVC.GetModel<PacketData>().InitPacketData();

        //初始化模型
        InitModel();

        CfgController.Instance.LoadAllCfg();
        
    }


    private void InitModel()
    {
        //背包模型
        InitDataAndRegisterModel<PacketData>();
        //扫荡模型
        InitDataAndRegisterModel<SweepData>();
    }

    /// <summary>
    /// 注册模型和初始化游戏数据
    /// </summary>
    private void InitDataAndRegisterModel<T>() where T : Model, new()
    {
        T model=new T();
        MVC.RegisterModel(model);
        model.InitModel();
    }

    // Update is called once per frame
    void Update () {
        
	}
}
