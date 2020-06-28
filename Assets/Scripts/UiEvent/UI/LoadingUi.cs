using System.Collections;
using System.Collections.Generic;
using UiCore;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingUi : View {

    public static string levelSceneName;
    private Slider loadSlider;
    private Text loadText;

    //异步加载的对象
    private AsyncOperation asyn;

    //场景加载实际进度
    private int trueLoading;
    //场景加载显示进度
    private int showLoading;

    protected override E_UIID uiid
    {
        get { return E_UIID.LoadingUi; }
    }

    public override void AddMessageListener()
    {
        
    }

    public override void RemoveMessageListener()
    {
        
    }

    //public override string Name
    //{
    //    get { return "LoadingUi"; }
    //}

    //public override void HandleEvent(string eventName, object data)
    //{
    //    throw new System.NotImplementedException();
    //}

    protected override void InitDataOnAwake()
    {
        //uiid = E_UIID.LoadingUi;
        uiType.ShowUiMode = E_ShowUIMode.HideAll;
        uiType.audio = E_Audio.NotPlay;
    }
    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();

        loadSlider = GameTool.GetTheChildComponent<Slider>(gameObject, "Slider");
        loadText = GameTool.GetTheChildComponent<Text>(gameObject, "LoadingText");


    }

    protected override void OnEnable()
    {
        base.OnEnable();
        showLoading = 0;//重新设置进度条的值
        levelSceneName = SceneController.Instance.sceneNameAsync;
        asyn = SceneManager.LoadSceneAsync(levelSceneName);
        asyn.allowSceneActivation = false;
        //allowSceneActivation默认值为True，场景加载完成后自动跳转场景
    }

    protected override void RegisterCtrl()
    {
        
    }

    void Update()
    {
        if (asyn != null)
        {
            //progress异步加载中场景加载的进度0-1,但是这个值对多只能到0.9
            if (asyn.progress < 0.9f)
            {
                trueLoading = (int)(asyn.progress * 100);
            }
            else//加载完成
            {
                trueLoading = 100;
            }

            if (showLoading < trueLoading)
            {
                showLoading++;
            }

            loadSlider.value = showLoading / 100f;
            if (loadSlider.value == 1)
            {
                asyn.allowSceneActivation = true;
                //UiManager.Instance.ShowUI(E_UIID.PlayUi);
                if (SceneController.Instance.handleAsync != null)
                {
                    SceneController.Instance.handleAsync();
                    SceneController.Instance.handleAsync = null;
                }
            }

            loadText.text = showLoading.ToString();
        }
    }
}
