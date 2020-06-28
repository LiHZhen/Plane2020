using UnityEngine;
using System.Collections;
//泛型
//不继承于MonoBehaviour的单例模式
public class Singleton<T>where T:new()
{
   //2声明一个私有的该类的类型的静态字段
    private static T instance;
   //1.私有化构造函数,这样外界就不能通过new关键字来实例化对象了
    protected Singleton()
    {

    }
    //3.定义一个共有的静态属性,返回值为该类的类型,让外部调用
    public static T Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }
}

//继承MonoBehaviour的单例模式
public class UnitySingleton<T> : MonoBehaviour where T:Component
{
    //会把继承了该类的脚本放在场景里面的一个空物体，用来挂载项目中所有继承于mono的单例
    private static GameObject go;
    //声明一个私有的该类的类型的静态字段
    private static T instance;
    //私有化构造函数,这样外界就不能通过new关键字来实例化对象了
    protected UnitySingleton()
    {

    }
    //定义一个共有的静态属性,返回值为该类的类型,让外部调用
    public static T Instance
    {
        get 
        {
            if (instance == null)
            {
                if (go == null)
                {
                    go = GameObject.Find("UnitySingletonObj");
                    //把单例模式的脚本挂载到go游戏物体上
                    if (go == null)
                    {
                        Debug.LogError("场景里找不到UnitySingletonObj物体");
                    }
                }
                instance = go.GetComponent<T>();
            }
            return instance;
        }
    }
}
