using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SendMessageCenter : MonoBehaviour {
    
    
    public delegate void DelCallBack();
    //委托：用来存放听到消息后要处理的逻辑
    public static Dictionary<E_MessageType,DelCallBack> dicMessageType=new Dictionary<E_MessageType, DelCallBack>();
    //字典：用来存放监听<消息类型，处理的逻辑>

    public static void AddMessageListener(E_MessageType messageType, DelCallBack handler)
    {
        if (!dicMessageType.ContainsKey(messageType))
        {
            //如果没有监听则添加到监听字典
            dicMessageType.Add(messageType,null);
        }
        dicMessageType[messageType] += handler;
    }

    //移除指定用户的指定监听的方法
    public static void RemoveMessageListener(E_MessageType messageType, DelCallBack handler)
    {
        if (dicMessageType.ContainsKey(messageType))
        {
            dicMessageType[messageType] -= handler;
        }
    }
    //移除指定指定用户的所有监听的方法，重载上一方法
    public static void RemoveMessageListener(DelCallBack handler)
    {
        if (dicMessageType.ContainsValue(handler))
        {
            foreach (var VARIABLE in dicMessageType)
            {
                
            }
        }
    }

    //移除指定消息类型所有监听者的方法
    public static void RemoveAllListenerByMessage(E_MessageType messageType)
    {
        if (dicMessageType.ContainsKey(messageType))
        {
            dicMessageType.Remove(messageType);
        }
    }

    //移除所有消息监听
    public static void RemoveAllListener()
    {
        dicMessageType.Clear();
    }

    //模拟广播消息
    public static void SendMessage(E_MessageType messageType)
    {
        DelCallBack del;
        if (dicMessageType.TryGetValue(messageType,out del))
        {
            if (del != null)
            {
                del();
            }
        }
    }

	void Start () {
		
	}
	

	void Update () {
		
	}
}
