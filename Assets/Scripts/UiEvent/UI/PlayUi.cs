using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UiCore;
using UnityEngine;
using UnityEngine.UI;

public class PlayUi : View
{
    private Button passBtn;
    private Button star1Btn;
    private Button star2Btn;
    private Button star3Btn;
    private Button notPassBtn;

    private GameObject overPanel;
    private Button backTolevelBtn;

    //三种星数的图片
    private Sprite spriteStar1;
    private Sprite spriteStar2;
    private Sprite spriteStar3;
    private Sprite spriteStar0;
    private Image overShowStarImg;

    //显示失败
    private Text getGiftText;

    protected override E_UIID uiid
    {
        get { return E_UIID.PlayUi; }
    }

    //    public override string Name
    //    {
    //        get
    //        {
    //            return "PlayUi";
    //        }
    //    }

    public override void AddMessageListener()
    {
        //添加监听
    }

    public override void RemoveMessageListener()
    {
        //移除监听
    }
    protected override void InitDataOnAwake()
    {
        //uiid = E_UIID.PlayUi;
        uiType.ShowUiMode = E_ShowUIMode.HideAll;
        uiType.UiRootType = E_UiRootType.Normal;
        uiType.audio = E_Audio.NotPlay;
        uiType.destroyType = E_DestroyType.ImmidiatelyDestroy;
    }

    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
        passBtn = GameTool.FindTheChild(gameObject, "PassBtn").GetComponent<Button>();
        notPassBtn = GameTool.FindTheChild(gameObject, "NotPassBtn").GetComponent<Button>();
        star1Btn = GameTool.FindTheChild(gameObject, "Star1").GetComponent<Button>();
        star2Btn = GameTool.FindTheChild(gameObject, "Star2").GetComponent<Button>();
        star3Btn = GameTool.FindTheChild(gameObject, "Star3").GetComponent<Button>();

        overPanel = GameTool.FindTheChild(gameObject, "OverPanel").gameObject;
        backTolevelBtn = GameTool.FindTheChild(gameObject, "BackToLevelBtn").GetComponent<Button>();

        spriteStar1 = Resources.Load<Sprite>("UiStar/SpriteStar1");
        spriteStar0 = Resources.Load<Sprite>("UiStar/SpriteStar0");
        spriteStar2 = Resources.Load<Sprite>("UiStar/SpriteStar2");
        spriteStar3 = Resources.Load<Sprite>("UiStar/SpriteStar3");
        overShowStarImg = GameTool.FindTheChild(gameObject, "OverShowStar").GetComponent<Image>();

        getGiftText = GameTool.FindTheChild(gameObject, "GetGiftText").GetComponent<Text>();
    }

    protected override void RegisterCtrl()
    {

    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Start()
    {
        passBtn.onClick.AddListener(delegate { PassLevel(0);});
        star1Btn.onClick.AddListener(delegate { PassLevel(1);});
        star2Btn.onClick.AddListener(delegate { PassLevel(2);});
        star3Btn.onClick.AddListener(delegate { PassLevel(3);});
        notPassBtn.onClick.AddListener(NotPassLevel);
        backTolevelBtn.onClick.AddListener(ShowLevelUi);
    }

    public void NotPassLevel()
    {
        
        getGiftText.text = "通关失败";
        overPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void PassLevel(int star)
    {
        //Debug.Log(star);
        //获取当前开启的最高关卡
        int maxLevel = LevelManager.Instance.GetCurrentMaxLevel();
        //当前进入的关卡
        int currentLevel = LevelManager.Instance.GetCurrentEnterLevel();
        if (LevelManager.Instance.GetStarCount(currentLevel) < star)
        {
            LevelManager.Instance.ChangeLevelStar(currentLevel, star);
        }
        if (LevelManager.Instance.GetCurrentEnterLevel() == maxLevel)
        {
            LevelManager.Instance.SetCurrentMaxLevel(++maxLevel);
        }
        GameManager.Instance.ShowAllReward();
        overPanel.SetActive(true);
        SetOverShowStar(star);
        Time.timeScale = 0;
    }

    /// <summary>
    /// 设置通关后的星级
    /// </summary>
    /// <param name="star"></param>
    private void SetOverShowStar(int star)
    {
        switch (star)
        {
            case 1:
                overShowStarImg.sprite = spriteStar1;
                break;
            case 2:
                overShowStarImg.sprite = spriteStar2;
                break;
            case 3:
                overShowStarImg.sprite = spriteStar3;
                break;
            default:
                overShowStarImg.sprite = spriteStar0;
                break;
        }
    }

    private void ShowLevelUi()
    {
        Time.timeScale = 1;
        getGiftText.text = "获得奖励";
        SceneController.Instance.LoadSceneAsync("Main", delegate
        {
            UiManager.Instance.ShowUI(E_UIID.LevelUi, false);
        });
        
    }



    //public override void HandleEvent(string eventName, object data)
    //{
    //    throw new NotImplementedException();
    //}
}
