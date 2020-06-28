using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    //速度
    private float speed = 2f;
    //死亡特效
    protected GameObject dieExplosion;
    //超出范围
    protected float overY = -5f;
    //死亡增加分数
    protected float score = 0f;
    protected float time;
    protected float shootCount=1.2f;
    public string enemyId = "0";
    protected string bulletName= "BulletRed";
    public float Score
    {
        get
        {
            return score;
        }

        set
        {
            score = value;
        }
    }

    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
        }
    }

    void Awake()
    {
        
    }

    // Use this for initialization
    void Start()
    {
        
    }

    protected virtual void InitOnStart()
    {
        dieExplosion = Resources.Load<GameObject>("GamePrefabs/Explosion/explosion_enemy");
        speed = CfgController.Instance.ReadEnemyCfg(enemyId).Speed;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void Move()
    {
        transform.Translate(Vector3.down * Speed * Time.deltaTime);

        //飞机超出范围
        if (transform.position.y <= overY)
        {
            Destroy(gameObject);
        }
    }


    protected void TriggerBullet(Collider collider)
    {
        if (collider.tag == "PlayerBullet")
        {
            //加分
            GameManager.Instance.GameScore += Score;
            //Debug.Log(GameManager.Instance.GameScore);
            Die();
        }
    }


    protected void Die()
    {
        GameObject exp = Instantiate(dieExplosion, transform.position, Quaternion.identity);
        Destroy(exp, 3);
        Destroy(gameObject);
    }

    protected void Attack()
    {
        time += Time.deltaTime;
        if (time >= 1 / shootCount)
        {
            GameObject bullet = ObjectPool.instance.Get(bulletName, new Vector3(transform.position.x, transform.position.y - 0.6f), Quaternion.Euler(0,0,180)) as GameObject;
            if (bullet.GetComponent<BulletMove>() == null)
            {
                bullet.AddComponent<BulletMove>();
            }

            time = 0;
        }
    }
}
