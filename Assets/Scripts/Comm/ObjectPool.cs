using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    private static Dictionary<string,ArrayList> pool=new Dictionary<string, ArrayList>();

    void OnEnable()
    {
        pool.Clear();
    }
	void Start ()
    {
        instance = this;
    }

    public Object Return(GameObject o)
    {
        string key = o.name;
        if (!pool.ContainsKey(key))
        {
            pool[key]=new ArrayList(){o};
        }
        else
        {
            pool[key].Add(o);
        }

        o.SetActive(false);
        return o;
    }
    //把生成物体改为
    //GameObject obj = ObjectPool.instance.Get("Resources文件夹里的名称", 位置, 旋转) as GameObject;
    //使用ObjectPools的Get方法
    public Object Get(string prefabName, Vector3 position, Quaternion quaternion)
    {
        string key = prefabName + "(Clone)";
        Object o;
        if (pool.ContainsKey(key) && pool[key].Count > 0)
        {
            ArrayList list = pool[key];
            o=list[0] as Object;
            list.RemoveAt(0);


            (o as GameObject).SetActive(true);
            (o as GameObject).transform.position = position;
            (o as GameObject).transform.rotation = quaternion;
        }
        else
        {
            o = Instantiate(Resources.Load("GamePrefabs/" + prefabName), position, quaternion);
        }
        return o;
    }
    //要在被加载的物体上添加代码
    //被创造出来时开启协程
    //void OnEnable()
    //{
    //    Init();
    //}
    //开启协程的方法
    //public void Init()
    //{
    //    StartCoroutine(ReturnToPool());
    //}
    //两秒钟调用一次对象池
    //IEnumerator ReturnToPool()
    //{
    //    yield return new WaitForSeconds(2f);
    //  清空原来的作用力和旋转力
    //  rb.angularVelocity = Vector3.zero;
    //  rb.velocity = Vector3.zero;
    //    ObjectPool.instance.Return(this.gameObject);
    //}
}
