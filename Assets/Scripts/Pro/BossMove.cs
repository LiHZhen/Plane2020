using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : BaseEnemy
{

    private Transform player;

	// Use this for initialization
	void Start () {
		base.InitOnStart();
        shootCount = 3f;
        bulletName = "BossBullet";
        player = GameObject.Find("PlayerShip(Clone)").transform;
    }
	
	// Update is called once per frame
	void Update () {
		base.Attack();
        Move();
	}

    protected override void Move()
    {
        float bossX = transform.position.x;
        float playerX = player.position.x;
        float x = 0;
        transform.Translate(Vector3.down * Speed * Time.deltaTime);
        if (transform.position.y <= 3.45f)
        {
            Speed = 0;
        }

        if (bossX!=playerX)
        {
            x=Mathf.Lerp(bossX,playerX,0.5f*Time.deltaTime);
        }
        transform.position=new Vector3(x,transform.position.y);
    }

    private void OnTriggerEnter(Collider collider)
    {
        base.TriggerBullet(collider);

    }
}
