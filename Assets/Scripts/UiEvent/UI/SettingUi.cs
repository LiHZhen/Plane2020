using System.Collections;
using System.Collections.Generic;
using UiCore;
using UnityEngine;
using UnityEngine.UI;

public class SettingUi : View
{

    private Button backBtn;

    protected override E_UIID uiid
    {
        get { return E_UIID.SettingUi; }
    }

    public override void AddMessageListener()
    {
        
    }

    public override void RemoveMessageListener()
    {
        
    }

    //    public override string Name
    //    {
    //        get
    //        {
    //            return "SettingUi";
    //        }
    //    }

    //public override void HandleEvent(string eventName, object data)
    //{
    //    throw new System.NotImplementedException();
    //}

    protected override void InitDataOnAwake()
    {
        //uiid = E_UIID.SettingUi;
        uiType.ShowUiMode = E_ShowUIMode.HideOther;
        uiType.UiRootType = E_UiRootType.Normal;
        uiType.audio = E_Audio.Play;
        uiType.destroyType = E_DestroyType.ImmidiatelyDestroy;
    }

    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();

        backBtn = GameTool.FindTheChild(gameObject, "BackBtn").GetComponent<Button>();
    }

    protected override void RegisterCtrl()
    {

    }
    protected override void Start()
    {
        backBtn.onClick.AddListener(ToBackClick);
    }
}
