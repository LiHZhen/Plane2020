using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Dictionary<string, GameObject> dicEnemyPrefab;
    private GameObject enemyPrefab;

    private float timer = 1f;
    private float startY = 5.5f;
    private float startX = 0;
    private int score=0;

    private Vector3 border_LeftTop;
    private Vector3 border_RightTop;

    //敌机的ID对应数量(该关卡会出现的敌机类型与数量)
    Dictionary<string, int> dicEnemy;

    private Transform enemyItem;
    

    void Awake()
    {
        enemyItem = GameObject.Find("EnemyItem").transform;

        border_LeftTop = CameraBorderTool.GetCameraBorder(E_BorderType.LeftTop, Camera.main);
        border_RightTop = CameraBorderTool.GetCameraBorder(E_BorderType.RightTop, Camera.main);
    }
	void Start ()
    {
        LoadEnemyPrefab();

        dicEnemy = new Dictionary<string, int>();
        LevelCfg lc = CfgController.Instance.ReadLevelCfg(LevelManager.Instance.GetCurrentEnterLevel().ToString());
        string enemyData = lc.Enemys;
        string[] allEnemyInfor = enemyData.Split(';');
        for (int i = 0; i < allEnemyInfor.Length; i++)
        {
            string[] enemyInfor = allEnemyInfor[i].Split(',');
            dicEnemy.Add(enemyInfor[0], int.Parse(enemyInfor[1]));
        }
        //开始生成敌机
        StartCoroutine(MakeEnemys());
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    

    private IEnumerator MakeEnemys()
    {
        string prefabName;
        foreach (KeyValuePair<string, int> item in dicEnemy)
        {
            //获取敌机的Id
            string id = item.Key;
            //获取敌机预制体的名称
            prefabName = CfgController.Instance.ReadEnemyCfg(id).PrefabName;
            
            
            for (int i = 0; i < item.Value; i++)
            {
                MakeSingleEnemy(prefabName,id);
                yield return new WaitForSeconds(timer);
            }
            
        }
        //enemyPrefab = Resources.Load<GameObject>("GamePrefab/Enemy/" + enemyName);
        //Debug.Log("over");
        GameManager.Instance.makeEnemysOver = true;
    }
    private void MakeSingleEnemy(string prefabName,string id)
    {
        enemyPrefab = dicEnemyPrefab[prefabName];
        //随机出生成的位置(-6.6~6.6)
        //startX = UnityEngine.Random.Range(-6.6f, 6.6f);
        startX = UnityEngine.Random.Range(border_LeftTop.x, border_RightTop.x);
        GameObject enemy = Instantiate(enemyPrefab, new Vector3(startX, startY, 0), Quaternion.identity) as GameObject;
        score = CfgController.Instance.ReadEnemyCfg(id).Score;
        
        switch (id)
        {
            case "1":
            case "2":
            case "3":
                enemy.AddComponent<EnemyMove>().Score = score;
                enemy.GetComponent<EnemyMove>().enemyId = id;
                break;
            case "4":
                enemy.AddComponent<RockMove>().Score = score;
                enemy.GetComponent<RockMove>().enemyId = id;
                break;
            case "5":
                enemy.AddComponent<BossMove>().Score = score;
                enemy.GetComponent<BossMove>().enemyId = id;
                break;
            default:
                Debug.Log("没有此ID敌人");
                break;
        }
        
        enemy.transform.SetParent(enemyItem);
    }
    private void LoadEnemyPrefab()
    {
        dicEnemyPrefab = new Dictionary<string, GameObject>();
        GameObject[] enemys = Resources.LoadAll<GameObject>("GamePrefabs/Enemy/");
        for (int i = 0; i < enemys.Length; i++)
        {
            dicEnemyPrefab.Add(enemys[i].name, enemys[i]);
        }
    }
}
