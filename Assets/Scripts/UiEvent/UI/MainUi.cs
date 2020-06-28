using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UiCore;
using UnityEngine;
using UnityEngine.UI;

public class MainUi : View
{

    private Button startBtn;
    private Button settingBtn;
    private Button exitBtn;
    private Button packetBtn;
    private Button shopBtn;
    private Button strengthenBtn;

    protected override E_UIID uiid
    {
        get { return E_UIID.MainUi; }
    }

    //public override string Name
    //{
    //    get { return "MainUi"; }
    //}

    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
        startBtn = GameTool.FindTheChild(gameObject, "StartBtn").GetComponent<Button>();
        settingBtn = GameTool.FindTheChild(gameObject, "SettingBtn").GetComponent<Button>();
        exitBtn = GameTool.FindTheChild(gameObject, "ExitBtn").GetComponent<Button>();
        packetBtn = GameTool.FindTheChild(gameObject, "PacketBtn").GetComponent<Button>();
        shopBtn = GameTool.FindTheChild(gameObject, "ShopBtn").GetComponent<Button>();
        strengthenBtn = GameTool.FindTheChild(gameObject, "StrengthenBtn").GetComponent<Button>();
    }

    protected override void InitDataOnAwake()
    {
        //uiid = E_UIID.MainUi;
        uiType.ShowUiMode = E_ShowUIMode.HideOther;
        uiType.UiRootType = E_UiRootType.Normal;
        uiType.audio = E_Audio.NotPlay;
        uiType.destroyType = E_DestroyType.NoDestroy;
    }

    protected override void Start()
    {
        base.Start();
        startBtn.onClick.AddListener(ToLevelUi);
        settingBtn.onClick.AddListener(SettingUiShow);
        exitBtn.onClick.AddListener(ExitGame);
        packetBtn.onClick.AddListener(ToPacketUi);
        shopBtn.onClick.AddListener(ToShopUi);
        strengthenBtn.onClick.AddListener(ShowStrengthenUi);
    }

    protected override void RegisterCtrl()
    {

    }

    private void ShowStrengthenUi()
    {
        UiManager.Instance.ShowUI(E_UIID.StrengthenUi);
    }

    private void ToShopUi()
    {
        UiManager.Instance.ShowUI(E_UIID.ShopUi);
    }

    private void ToPacketUi()
    {
        UiManager.Instance.ShowUI(E_UIID.PacketUi);
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void SettingUiShow()
    {
        UiManager.Instance.ShowUI(E_UIID.SettingUi);
    }

    private void ToLevelUi()
    {
        UiManager.Instance.ShowUI(E_UIID.LevelUi);
    }

    public override void AddMessageListener()
    {

    }

    public override void RemoveMessageListener()
    {

    }

    //public override void HandleEvent(string eventName, object data)
    //{
    //    throw new NotImplementedException();
    //}
}
