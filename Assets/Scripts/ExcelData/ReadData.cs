using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadData : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DataController.Instance.LoadAllCfg();
        //加载
        //string str=DataController.Instance.ReadCfg("Name", 3, DataController.Instance.dicPack);
        //string str = CfgController.Instance.ReadItemCfg("3").Name;
        //读取配置表
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
