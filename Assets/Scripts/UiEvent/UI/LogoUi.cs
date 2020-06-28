using System.Collections;
using System.Collections.Generic;
using UiCore;
using UnityEngine;

public class LogoUi : View {
    protected override E_UIID uiid
    {
        get { return E_UIID.LogoUi; }
    }

    //public override string Name
    //{
    //    get { return "LogoUi"; }
    //}

    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
    }

    protected override void InitDataOnAwake()
    {
        //uiid = E_UIID.LogoUi;
        uiType.ShowUiMode = E_ShowUIMode.DoNothing;
        uiType.UiRootType = E_UiRootType.Normal;
        uiType.audio = E_Audio.NotPlay;
        uiType.destroyType = E_DestroyType.ImmidiatelyDestroy;
    }

    public override void AddMessageListener()
    {
        
    }

    public override void RemoveMessageListener()
    {
        
    }

    protected override void RegisterCtrl()
    {
        
    }

    //public override void HandleEvent(string eventName, object data)
    //{
    //    throw new System.NotImplementedException();
    //}
}
