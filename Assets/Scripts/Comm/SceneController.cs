using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{

    public string sceneNameAsync;//将要异步加载的场景名称
    public Action handleAsync;//储存异步加载后要处理的逻辑

    /// <summary>
    /// 直接加载场景的方法
    /// </summary>
    /// <param name="name">要加载的场景的名称</param>
    /// <param name="hander">场景加载完要处理的逻辑</param>
    public void LoadScene(string name,Action hander=null)
    {
        SceneManager.LoadScene(name);
        if (hander != null)
        {
            hander();
        }
    }
    //异步加载方法
    public void LoadSceneAsync(string name, Action hander = null)
    {
        sceneNameAsync = name;
        handleAsync = hander;
        SceneManager.LoadSceneAsync("LoadingScene");
        UiManager.Instance.ShowUI(E_UIID.LoadingUi);
    }
}
