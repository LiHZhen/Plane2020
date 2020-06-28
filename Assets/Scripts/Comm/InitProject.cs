using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//动画播放完，场景加载完，这个时候才可以切换到MainScene
public class InitProject : MonoBehaviour
{
    //logo播放完动画所需的时间
    private float showTime = 6;
    //计时器
    private float time = 0;
    //异步加载的对象
    AsyncOperation async;
    // Use this for initialization
    void Start()
    {

        async = SceneManager.LoadSceneAsync("Test");
        async.allowSceneActivation = false;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= showTime && async.progress >= 0.9f)
        {
            async.allowSceneActivation = true;
        }

    }
}
