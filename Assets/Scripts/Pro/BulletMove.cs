using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    private float speed=20f;
    private Rigidbody rb;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
	void Start () {
		
	}
	
	
	void Update () {
		transform.Translate(Vector3.up*speed*Time.deltaTime);
	}

    void OnEnable()
    {
        Init();
    }
    //开启协程的方法
    public void Init()
    {
        StartCoroutine(ReturnToPool());
    }
    //两秒钟调用一次对象池
    IEnumerator ReturnToPool()
    {
        yield return new WaitForSeconds(2f);
        //清空原来的作用力和旋转力
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        ObjectPool.instance.Return(this.gameObject);
    }
}
