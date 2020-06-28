//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class GameDebug : MonoBehaviour//封装一个Debug，加一个开关方便使用
//{
//    public static bool isOn = true;

//    public static void Log(object message)
//    {
//        Log(message,null);
//    }

//    public static void Log(object message, Object obj)
//    {
//        if (isOn)
//        {
//            Debug.Log(message,obj);
//        }
//    }

//    public static void LogWarning(object message)
//    {
//        Log(message, null);
//    }

//    public static void LogWarning(object message, Object obj)
//    {
//        if (isOn)
//        {
//            Debug.LogWarning(message, obj);
//        }
//    }

//    public static void LogError(object message)
//    {
//        Log(message, null);
//    }

//    public static void LogError(object message, Object obj)
//    {
//        if (isOn)
//        {
//            Debug.LogError(message, obj);
//        }
//    }
//}
