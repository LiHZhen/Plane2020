using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMove : BaseEnemy
{

	// Use this for initialization
	void Start () {
        base.InitOnStart();
    }
	
	// Update is called once per frame
	void Update () {
        base.Move();
    }

    private void OnTriggerEnter(Collider collider)
    {
        base.TriggerBullet(collider);

    }
}
