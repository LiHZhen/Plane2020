using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEntity : MonoBehaviour
{

    public int levelId = 0;

    private Button levelBtn;

    private Image starImg;

    private LevelUi levelUi;


    class LevelStar
    {
        public string starId;
        public int star;
    }

    void Awake()
    {
        levelBtn =gameObject.GetComponent<Button>();
        starImg = GameTool.FindTheChild(gameObject, "StarImg").GetComponent<Image>();
        levelUi = GameObject.Find("LevelUi(Clone)").GetComponent<LevelUi>();
    }
    void Start()
    {
        levelBtn.onClick.AddListener(LoadLevelListener);
        levelBtn.onClick.AddListener(levelUi.SweepBtnClick);
        levelBtn.onClick.AddListener(levelUi.ShowPassGift);
        levelBtn.onClick.AddListener(levelUi.InitSweepCount);

    }

    private void LoadLevelListener()
    {
        LevelManager.Instance.SetCurrentEnterLevel(levelId);//设置进入关卡的id
        //string levelName = DataController.Instance.ReadCfg("LevelFileName", levelId, DataController.Instance.dicLevel);
        //string levelName = CfgController.Instance.ReadLevelCfg(levelId.ToString()).LevelFileName;
        string levelName = "SceneLevel1";
        //SceneManager.LoadScene("LoadingScene");
        //LoadingUi.levelSceneName = levelName;
        //把关卡名称传给LevelUi，让他点击按钮再切换场景
        LevelUi ui = GameObject.Find("LevelUi(Clone)").GetComponent<LevelUi>();
        ui.SetLevelSceneName(levelName);

        
    }
}
