using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

//控制器，用来处理对应的逻辑
namespace UiCore
{

    public abstract class Controller:UnitySingleton<Controller>
    {

        /// <summary>
        /// 获取模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetModel<T>() where T : Model
        {
            return MVC.GetModel<T>();
        }

        /// <summary>
        /// 获取视图
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetView<T>() where T : View,new()
        {
            return UiManager.Instance.GetUiScript<T>();
        }

        /// <summary>
        /// 注册模型
        /// </summary>
        /// <param name="model">模型类型</param>
        protected void RegisterModel(Model model)
        {
            MVC.RegisterModel(model);
        }

        /// <summary>
        /// 注册视图
        /// </summary>
        /// <param name="view">视图类型</param>
        //protected void RegisterView(View view)
        //{
        //    MVC.RegisterView(view);
        //}

        /// <summary>
        /// 注册控制器（自身）
        /// </summary>
        /// <param name="eventName">控制器名称</param>
        /// <param name="controllerType">控制器类型</param>
        protected void RegisterController(E_CommandType eventName, Type controllerType)
        {
            MVC.RegisterController(eventName, controllerType);
        }

        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="data"></param>
        public abstract void Execute(object data1,object data2);
    }

}