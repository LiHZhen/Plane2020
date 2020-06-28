using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UiCore
{
    /// <summary>
    /// MVC管理类
    /// </summary>
    public class MVC
    {

        //储存MVC 3层
        // 储存<模型名称,模型>
        public static Dictionary<string, Model> Models = new Dictionary<string, Model>();

        // 储存<视图名称,视图> 用了UiManager所以不用储存视图
        //public static Dictionary<E_UIID, View> Views = new Dictionary<E_UIID, View>();

        // 储存<命令名称,控制器类型>
        public static Dictionary<E_CommandType, Type> CommadMap = new Dictionary<E_CommandType, Type>();

        //注册相关↓

        /// <summary>
        /// 注册模型
        /// </summary>
        /// <param name="model">模型</param>
        public static void RegisterModel(Model model)
        {
            if (!Models.ContainsKey(model.Name))
            {
                Models.Add(model.Name, model);
            }
        }

        /// <summary>
        /// 注册视图
        /// </summary>
        /// <param name="view">视图</param>
        //public static void RegisterView(View view)
        //{
        //    if (!Views.ContainsKey(view.Uiid))
        //    {
        //        Views.Add(view.Uiid, view);
        //        //注册视图后还要注册所要监听的消息
        //        //view.RegisterEvent();
        //    }
        //}

        /// <summary>
        /// 注册控制器
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="controllerType"></param>
        public static void RegisterController(E_CommandType commandType, Type controllerType)
        {
            if (!CommadMap.ContainsKey(commandType))
            {
                CommadMap.Add(commandType, controllerType);
            }
        }

        //获取相关↓

        /// <summary>
        /// 获取模型
        /// </summary>
        /// <typeparam name="T">模型类型</typeparam>
        /// <returns></returns>
        public static T GetModel<T>() where T : Model
        {
            foreach (Model m in Models.Values)
            {
                if (m is T)
                {
                    return (T) m;
                }
            }

            return null;
        }
        //public static T GetModel<T>() where T : Model,new()
        //{
        //    string modelName = new T().Name;
        //    if (Models.ContainsKey(modelName))
        //    {
        //        return (T) Models[modelName];
        //    }

        //    return null;
        //}

        /// <summary>
        /// 获取视图
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <returns></returns>
        //public static T GetView<T>() where T : View
        //{
        //    foreach (View v in Views.Values)
        //    {
        //        if (v is T)
        //        {
        //            return (T) v;
        //        }
        //    }

        //    return null;
        //}

        /// <summary>
        /// 发送事件命令(触发)  模型->视图（消息）/  视图->控制器（命令）
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="data"></param>
        public static void SendCommand(E_CommandType eventName, object data1 = null,object data2=null)
        {
            //eventName如果是命令，控制器响应事件
            if (CommadMap.ContainsKey(eventName))
            {
                Type t = CommadMap[eventName];
                //创建一个泛型参数所属类型的对象
                Controller ctrl = Activator.CreateInstance(t) as Controller;
                //执行控制器
                ctrl.Execute(data1,data2);
            }

            //eventName如果是消息，视图响应事件
            //foreach (View v in Views.Values)
            //{
            //    if (v.AttentionEvents.Contains(eventName))
            //    {
            //        v.HandleEvent(eventName, data);
            //    }

            //}
        }
    }
}