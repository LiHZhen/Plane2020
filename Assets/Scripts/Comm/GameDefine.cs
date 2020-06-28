using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//窗体显示类型
//把所有枚举写在一个类里



public enum E_MessageType//消息类型
{
    //更新金币
    UpdateCoin,
    InitPacket,
}

public enum E_CommandType //命令类型
{
    UpdateCoinCmd,
    //初始化背包
    InitPacketCmd,
    //翻页
    ChangePageCmd,
    //扫荡
    LevelSweepCmd,
    //初始化关卡
    InitLevelCmd,
}

public enum E_BorderType//游戏窗口边界
{
    LeftTop,
    LeftBottom,
    RightTop,
    RightBottom,
}


public enum E_ShowUIMode//显示界面时需要隐藏窗体的类型
{
    //界面显示时不需要隐藏其他窗体
    DoNothing,
    //界面显示时需要隐藏其他窗体
    HideOther,
    //界面显示时需要隐藏全部窗体
    HideAll
}

public enum E_UiRootType//是否显示在最前方的窗体类型
{
    //保持在屏幕前方的窗体(对应DoNothing)
    KeepAbove,
    //普通的窗体(对应HideOther,HideAll)
    Normal
}

public enum E_Audio//显示窗体时是否播放声音
{
    Play,
    NotPlay
}

public enum E_DestroyType//窗体的销毁类型
{
    NoDestroy,
    ImmidiatelyDestroy,
    Delay
}

public enum E_UIID//UI的所有ID
{
    NullUi,
    MainUi,
    LogoUi,
    LevelUi,
    PlayUi,
    LoadingUi,
    SettingUi,
    PacketUi,
    ShopUi,
    StrengthenUi,
}

public class GameDefine : MonoBehaviour
{
    //uiid对应预制体路径
    public static Dictionary<E_UIID, string> dicPath = new Dictionary<E_UIID, string>()
    {
        { E_UIID.MainUi,"UIPrefabs/MainUI"},
        { E_UIID.LogoUi,"UIPrefabs/LogoUi"},
        { E_UIID.LevelUi,"UIPrefabs/LevelUi"},
        { E_UIID.PlayUi,"UIPrefabs/PlayUi"},
        { E_UIID.LoadingUi,"UIPrefabs/LoadingUi"},
        { E_UIID.SettingUi,"UIPrefabs/SettingUi"},
        {E_UIID.PacketUi,"UIPrefabs/PacketUi"},
        {E_UIID.ShopUi,"UIPrefabs/ShopUi"},
        {E_UIID.StrengthenUi,"UIPrefabs/StrengthenUi"},
    };

    public static Type GetUiScriptType(E_UIID uiid)
    {
        Type scriptType = null;
        switch (uiid)
        {
            case E_UIID.MainUi:
                scriptType = typeof(MainUi);
                break;
            case E_UIID.LogoUi:
                scriptType = typeof(LogoUi);
                break;
            case E_UIID.LevelUi:
                scriptType = typeof(LevelUi);
                break;
            case E_UIID.PlayUi:
                scriptType = typeof(PlayUi);
                break;
            case E_UIID.LoadingUi:
                scriptType = typeof(LoadingUi);
                break;
            case E_UIID.SettingUi:
                scriptType = typeof(SettingUi);
                break;
            case E_UIID.PacketUi:
                scriptType = typeof(PacketUi);
                break;
            case E_UIID.ShopUi:
                scriptType = typeof(ShopUi);
                break;
            case E_UIID.StrengthenUi:
                scriptType = typeof(StrengthenUi);
                break;

            default:
                break;
        }

        return scriptType;
    }

}
