using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : BaseEnemy {
    ////速度
    //private float speed = 2f;
    ////死亡特效
    //private GameObject dieExplosion;
    ////超出范围
    //private float overY = -5f;
    ////死亡增加分数
    //private float score = 0f;
    //public float Score
    //{
    //    get
    //    {
    //        return score;
    //    }

    //    set
    //    {
    //        score = value;
    //    }
    //}

    void Awake()
    {
        
    }

    // Use this for initialization
    void Start () {
        base.InitOnStart();
        bulletName = "BulletRed";

    }
	
	// Update is called once per frame
	void Update () {
        base.Move();
        base.Attack();
    }

    private void OnTriggerEnter(Collider collider)
    {
        base.TriggerBullet(collider);

    }

}
