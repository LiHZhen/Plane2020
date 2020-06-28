using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UiCore;
using UnityEngine;
using UnityEngine.UI;



public enum E_LevelCmdType
{
    ShowLevelOnEnable,//初始化关卡列表
    PlayGame,//点击开始游戏
    SweepBtnClick,//点击扫荡按钮
    InitSweep,//初始化扫荡
    SweepEnter,//确定开始扫荡
    ShowPassGive,//显示扫荡奖励
}


public class LevelUi : View
{
    //储存关卡奖励的字典<id,数量>
    public Dictionary<string, int> dicReward = new Dictionary<string, int>();
    //转载关卡的物体
    public GameObject levelContent;
    //需要进行加载的物体
    public GameObject levelBtn;

    private Button playBtn;
    private string levelSceneName;

    private Button backBtn;

    //三种星数的图片
    public Sprite spriteStar1;
    public Sprite spriteStar2;
    public Sprite spriteStar3;

    //扫荡
    public Text myPrice;
    public Text sweepPrice;
    public Text sweepTipsText;
    private Button sweepBtn;
    public Button enterBtn;
    public int level;
    public GameObject tipsPanel;
    public Text tipsText;

    public int sweepCount = 3;
    public Text sweepTimesOneDay;
    //保存关卡扫荡次数字典<关卡id,扫荡次数>
    //private Dictionary<string, int> dicSweep = new Dictionary<string, int>();

    //通关奖励的提前显示
    public Transform giftContent;
    public GameObject onceGift;
    public Text giftNameText;
    public Text giftCountText;

    protected override E_UIID uiid
    {
        get { return E_UIID.LevelUi; }
    }

    public void SetLevelSceneName(string value)
    {
        levelSceneName = value;
    }

    protected override void InitDataOnAwake()
    {
        //uiid = E_UIID.LevelUi;
        uiType.ShowUiMode = E_ShowUIMode.HideOther;
        uiType.UiRootType = E_UiRootType.Normal;
        uiType.audio = E_Audio.NotPlay;
        uiType.destroyType = E_DestroyType.NoDestroy;
    }

    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
        levelContent = GameTool.FindTheChild(gameObject, "LevelContent").gameObject;
        levelBtn = Resources.Load("UIPrefabs/LevelBtn") as GameObject;
        playBtn = GameTool.FindTheChild(gameObject, "PlayBtn").GetComponent<Button>();
        backBtn = GameTool.FindTheChild(gameObject, "BackBtn").GetComponent<Button>();
        spriteStar1 = Resources.Load<Sprite>("UiStar/SpriteStar1");
        spriteStar2 = Resources.Load<Sprite>("UiStar/SpriteStar2");
        spriteStar3 = Resources.Load<Sprite>("UiStar/SpriteStar3");

        myPrice = GameTool.FindTheChild(gameObject, "MyPrice").GetComponent<Text>();
        sweepPrice = GameTool.FindTheChild(gameObject, "SweepPrice").GetComponent<Text>();
        sweepTipsText = GameTool.FindTheChild(gameObject, "SweepTipsText").GetComponent<Text>();
        sweepBtn = GameTool.FindTheChild(gameObject, "SweepBtn").GetComponent<Button>();
        enterBtn = GameTool.FindTheChild(gameObject, "EnterBtn").GetComponent<Button>();

        giftContent = GameTool.FindTheChild(gameObject, "GiftContent");
        onceGift = Resources.Load("UIPrefabs/OnceGiftJustText") as GameObject;
        tipsPanel= GameTool.FindTheChild(gameObject, "TipsPanel").gameObject;
        tipsText= GameTool.FindTheChild(tipsPanel, "TipsText").GetComponent<Text>();
        sweepTimesOneDay = GameTool.FindTheChild(enterBtn.gameObject, "SweepTimesOneDay").GetComponent<Text>();

        InitSweepCount();
    }



    protected override void Start()
    {
        base.Start();
        playBtn.onClick.AddListener(PlayGame);
        backBtn.onClick.AddListener(ToBackClick);
        sweepBtn.onClick.AddListener(SweepBtnClick);
        enterBtn.onClick.AddListener(SweepEnter);
    }


    protected override void RegisterCtrl()
    {
        //MVC.RegisterController(E_CommandType.PlayBtnCmd,typeof(LevelCommand));
        MVC.RegisterController(E_CommandType.InitLevelCmd,typeof(LevelCommand));
        MVC.RegisterController(E_CommandType.LevelSweepCmd,typeof(LevelSweepCommand));
    }

    

    protected override void OnEnable()
    {
        SendCommand(E_CommandType.InitLevelCmd,null,this);
    }

    private void PlayGame()
    {
        SendCommand(E_CommandType.InitLevelCmd,E_LevelCmdType.PlayGame);
    }
    /// <summary>
    /// 点击扫荡按钮
    /// </summary>
    public void SweepBtnClick()
    {
        SendCommand(E_CommandType.LevelSweepCmd,E_LevelCmdType.SweepBtnClick);
    }
    /// <summary>
    /// 初始化扫荡次数
    /// </summary>
    public void InitSweepCount()
    {
        SendCommand(E_CommandType.LevelSweepCmd, E_LevelCmdType.InitSweep,this);
    }
    /// <summary>
    /// 开始扫荡
    /// </summary>
    private void SweepEnter()
    {
        SendCommand(E_CommandType.LevelSweepCmd, E_LevelCmdType.SweepEnter);
    }

    /// <summary>
    /// 显示通关奖励
    /// </summary>
    public void ShowPassGift()
    {
        SendCommand(E_CommandType.LevelSweepCmd, E_LevelCmdType.ShowPassGive);
    }




    public override void AddMessageListener()
    {
        
    }

    public override void RemoveMessageListener()
    {
        
    }



}
