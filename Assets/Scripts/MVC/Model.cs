using System.Collections;
using System.Collections.Generic;
using System.IO;
using GameCore;
using LitJson;
using UnityEngine;
//主要用来储存数据的
namespace UiCore
{
    public abstract class Model
    {


        //T data=new T();

        /// <summary>
        /// 模型名称
        /// </summary>
        public abstract string Name { get; }

        public string json = null;

        public abstract string Path { get; }

        public void InitDataType<T>()
        {

        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <typeparam name="T">该模型存储数据的类型</typeparam>
        /// <param name="data"></param>
        public virtual void InitModel()
        {

            if (!File.Exists(Path))
            {
                //SaveData(data);
            }
            else
            {
                CoroutineController.Instance.StartCoroutine(LoadData());
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public void SaveData<T>(T data)
        {
            string jsonData = JsonMapper.ToJson(data);
            using (StreamWriter sw = new StreamWriter(Path))
            {
                sw.WriteLine(jsonData);
            }
        }

        protected abstract void JsonToObj();

        /// <summary>
        /// 根据数据加载Json文件
        /// </summary>
        /// <returns></returns>
        protected IEnumerator LoadData()
        {
            WWW www = new WWW("file:///" + Path);
            yield return www;
            json = www.text;
            JsonToObj();
        }

        #region 发送消息
        protected void SendMessage(E_MessageType messageType)
        {
            EventDispatcher.TriggerEvent(messageType);
        }
        protected void SendMessage<T>(E_MessageType messageType, T arg1)
        {
            EventDispatcher.TriggerEvent(messageType, arg1);
        }
        protected void SendMessage<T, U>(E_MessageType messageType, T arg1, U arg2)
        {
            EventDispatcher.TriggerEvent(messageType, arg1, arg2);
        }
        protected void SendMessage<T, U, V>(E_MessageType messageType, T arg1, U arg2, V arg3)
        {
            EventDispatcher.TriggerEvent(messageType, arg1, arg2, arg3);
        }
        protected void SendMessage<T, U, V, W>(E_MessageType messageType, T arg1, U arg2, V arg3, W arg4)
        {
            EventDispatcher.TriggerEvent(messageType, arg1, arg2, arg3, arg4);
        }  
        #endregion

    }
}