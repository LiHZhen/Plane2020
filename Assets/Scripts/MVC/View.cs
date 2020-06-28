using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace UiCore
{
    //窗体的类型
    public class UiType
    {
        //设置默认值，取最常用的
        public E_ShowUIMode ShowUiMode = E_ShowUIMode.HideOther;
        public E_UiRootType UiRootType = E_UiRootType.Normal;

        public E_Audio audio = E_Audio.Play;

        //销毁方式
        public E_DestroyType destroyType = E_DestroyType.NoDestroy;
    }

    public abstract class View : MonoBehaviour
    {
        
        /// <summary>
        /// 视图名称
        /// </summary>
        //public abstract string Name { get; }

//        /// <summary>
//        /// 监听的消息列表
//        /// </summary>
//        public List<string> AttentionEvents = new List<string>();
//
//        /// <summary>
//        /// 注册所要监听的消息
//        /// </summary>
//        public virtual void RegisterEvent()
//        {
//
//        }
//
//        /// <summary>
//        /// 事件处理函数（监听到消息以后所要处理的逻辑）
//        /// </summary>
//        /// <param name="eventName">消息名称</param>
//        /// <param name="data">要处理的逻辑</param>
//        public abstract void HandleEvent(string eventName, object data);

        /// <summary>
        /// 获取模型
        /// </summary>
        /// <typeparam name="T">模型类型</typeparam>
        /// <returns></returns>
        protected T GetModel<T>() where T : Model
        {
            return MVC.GetModel<T>();
        }

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="eventName">命令名称</param>
        /// <param name="data">参数</param>
        protected void SendCommand(E_CommandType eventName, object data1 = null,object data2=null)
        {
            //发送命令（触发）
            MVC.SendCommand(eventName, data1,data2);
        }

        public UiType uiType;

        //当前窗体的ID
        protected abstract E_UIID uiid { get; }

        public E_UIID Uiid
        {
            get { return uiid; }
        }

        //上一个跳转过来的窗体ID
        protected E_UIID beforeUiid = E_UIID.NullUi;

        public E_UIID BeforeUiid { get; set; }

        //延迟销毁时间
        public int destroyTime = 5;

        //计算销毁时间的计时器
        public Stopwatch destroyTimer;

        //声明一个属性判断窗体显示出来的时候是否要处理其他窗体
        public bool IsNeedDealWithUi
        {
            get
            {
                if (this.uiType.UiRootType == E_UiRootType.KeepAbove)
                {
                    return false;
                }

                return true;
            }
        }


        public virtual void Awake()
        {
            if (uiType == null)
            {
                uiType = new UiType();
            }

            InitUiOnAwake();
            InitDataOnAwake();
            destroyTimer = new Stopwatch();
        }

        //初始化界面元素（查找界面元素，获取监听等逻辑代码）
        protected virtual void InitUiOnAwake()
        {
            RegisterCtrl();
        }

        //初始化界面数据（窗体的ID，类型）
        protected abstract void InitDataOnAwake();

        /// <summary>
        /// 注册控制器
        /// </summary>
        protected abstract void RegisterCtrl();


        //显示窗体的方法
        public virtual void ShowUI()
        {
            this.gameObject.SetActive(true);
        }

        public virtual void HideUI(Action del = null)
        {
            this.gameObject.SetActive(false);
            if (del != null)
            {
                del();
            }

            //保存数据
            Save();
        }

        protected virtual void Save() //保存数据
        {

        }

        //显示窗体播放音效
        protected virtual void PlayAudio()
        {
            AudioManager.Instance.PlayEffectMusic("UIShow");
        }

        protected virtual void OnEnable()
        {
            //if (this.uiType.audio == E_Audio.Play)
            //{
            //    PlayAudio();
            //}
        }

        protected virtual void OnDisable()
        {
            //重置计时器
            destroyTimer.Reset();
            //开始计时
            destroyTimer.Start();
        }

        protected virtual void Start()
        {

            AddMessageListener();
        }

        protected virtual void OnDestroy()
        {
            RemoveMessageListener();
            if (uiType.destroyType == E_DestroyType.Delay)
            {
                destroyTimer.Stop();
            }

        }

        /// <summary>
        /// 注册所要监听的消息
        /// </summary>
        public abstract void AddMessageListener();
        /// <summary>
        /// 移除所要监听的消息
        /// </summary>
        public abstract void RemoveMessageListener();


        protected virtual void ToBackClick()
        {
            UiManager.Instance.ShowUI(BeforeUiid, false);
        }

        protected virtual void ToCloseClick()
        {
            UiManager.Instance.HideSingleUI(uiid);
        }

        //#region 添加监听
        //protected void AddListener(E_MessageType message, Action hander)
        //{
        //    EventDispatcher.AddListener(message, hander);
        //}
        //protected void AddListener<T>(E_MessageType message, Action<T> hander)
        //{
        //    EventDispatcher.AddListener(message, hander);
        //}
        //protected void AddListener<T, U>(E_MessageType message, Action<T, U> hander)
        //{
        //    EventDispatcher.AddListener(message, hander);
        //}
        //protected void AddListener<T, U, V>(E_MessageType message, Action<T, U, V> hander)
        //{
        //    EventDispatcher.AddListener(message, hander);
        //}
        //protected void AddListener<T, U, V, W>(E_MessageType message, Action<T, U, V, W> hander)
        //{
        //    EventDispatcher.AddListener(message, hander);
        //}
        //#endregion

        //#region 移除监听
        //protected void RemoveListener(E_MessageType message, Action hander)
        //{
        //    EventDispatcher.RemoveListener(message, hander);
        //}
        //protected void RemoveListener<T>(E_MessageType message, Action<T> hander)
        //{
        //    EventDispatcher.RemoveListener(message, hander);
        //}
        //protected void RemoveListener<T, U>(E_MessageType message, Action<T, U> hander)
        //{
        //    EventDispatcher.RemoveListener(message, hander);
        //}
        //protected void RemoveListener<T, U, V>(E_MessageType message, Action<T, U, V> hander)
        //{
        //    EventDispatcher.RemoveListener(message, hander);
        //}
        //protected void RemoveListener<T, U, V, W>(E_MessageType message, Action<T, U, V, W> hander)
        //{
        //    EventDispatcher.RemoveListener(message, hander);
        //}
        //#endregion
    }
}
