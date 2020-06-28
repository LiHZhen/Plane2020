using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipMove : MonoBehaviour
{
    private float moveSpeed=7;
    //一秒射击多少次
    private float shootCount = 8;
    //计时器
    private float time=0;

    private GameObject dieExp;

    private PlayUi playUi;

    private bool isAttack = false;
    //技能
    private bool isSkill = false;
    private float skillTime=0;
    private float resetSkillTime = 3f;

    public float ResetSkillTime
    {
        get
        {
            return resetSkillTime;
        }

        set
        {
            resetSkillTime = value;
        }
    }

    private Vector3 border_LeftTop;
    private Vector3 border_LeftBottom;
    private Vector3 border_RightTop;
    private Vector3 border_RightBottom;

    void Awake ()
    {
        dieExp = Resources.Load<GameObject>("GamePrefabs/Explosion/explosion_player");
        border_LeftTop = CameraBorderTool.GetCameraBorder(E_BorderType.LeftTop, Camera.main);
        border_LeftBottom = CameraBorderTool.GetCameraBorder(E_BorderType.LeftBottom, Camera.main);
        border_RightTop = CameraBorderTool.GetCameraBorder(E_BorderType.RightTop, Camera.main);
        border_RightBottom = CameraBorderTool.GetCameraBorder(E_BorderType.RightBottom, Camera.main);
    }
	
	void Update ()
    {
        Move();
        SkillStart();
        
        Attack();
    }

    private void SkillStart()
    {
        if (skillTime <= 0)
        {
            isSkill = false;
            if (ETCInput.GetButtonDown("Skill"))
            {
                skillTime += ResetSkillTime;
                isSkill = true;
            }
        }
        else
        {
            skillTime -= Time.deltaTime;
        }
        

    }

    private void Move()
    {
        float x = ETCInput.GetAxis("Horizontal");
        float y = ETCInput.GetAxis("Vertical");
        transform.Translate(new Vector3(x * Time.deltaTime * moveSpeed, y * Time.deltaTime * moveSpeed, 0));

        /*
        //x:+-6.5  ,y:-2.6,4.5  移动范围
        if (transform.position.x <= -6.5f)
        {
            transform.position=new Vector3(-6.5f,transform.position.y);
        }
        else if (transform.position.x >= 6.5f)
        {
            transform.position = new Vector3(6.5f, transform.position.y);
        }
        if (transform.position.y <= -2.6f)
        {
            transform.position = new Vector3(transform.position.x, -2.6f);
        }
        else if (transform.position.y >= 4.5f)
        {
            transform.position = new Vector3(transform.position.x, 4.5f);
        }
        */
        if (transform.position.x <= border_LeftTop.x)
        {
            transform.position = new Vector3(border_LeftTop.x, transform.position.y);
        }
        else if (transform.position.x >= border_RightTop.x)
        {
            transform.position = new Vector3(border_RightTop.x, transform.position.y);
        }
        if (transform.position.y <= border_LeftBottom.y)
        {
            transform.position = new Vector3(transform.position.x, border_LeftBottom.y);
        }
        else if (transform.position.y >= border_LeftTop.y)
        {
            transform.position = new Vector3(transform.position.x, border_LeftTop.y);
        }

    }

    private void Attack()
    {
        if (ETCInput.GetButtonDown("Attack"))
        {
            isAttack = true;
        }
        else if (ETCInput.GetButtonUp("Attack"))
        {
            isAttack = false;
        }
        if (isAttack)
        {
            time += Time.deltaTime;
            if (time >= 1 / shootCount)
            {
                //判断技能是否开启
                if (isSkill)
                {
                    GameObject bullet2 = ObjectPool.instance.Get("BulletGreen", new Vector3(transform.position.x+0.5f, transform.position.y), Quaternion.identity) as GameObject;
                    GameObject bullet3 = ObjectPool.instance.Get("BulletGreen", new Vector3(transform.position.x-0.5f, transform.position.y), Quaternion.identity) as GameObject;
                    AddBulletSprite(bullet2);
                    AddBulletSprite(bullet3);
                }
                GameObject bullet = ObjectPool.instance.Get("BulletGreen", new Vector3(transform.position.x, transform.position.y + 0.6f), Quaternion.identity) as GameObject;
                AddBulletSprite(bullet);
                time = 0;
            }
        }
    }

    private void AddBulletSprite(GameObject bullet)
    {
        if (bullet.GetComponent<BulletMove>() == null)
        {
            bullet.AddComponent<BulletMove>();
        }
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag.Equals("Enemy"))
        {
            playUi=GameObject.Find("PlayUi(Clone)").GetComponent<PlayUi>();
            StartCoroutine(NotPass());
            GameObject exp = Instantiate(dieExp,transform.position,Quaternion.identity);
            Destroy(exp,3);
            //gameObject.SetActive(false);
            Destroy(gameObject,3);
            dieExp = null;
            moveSpeed = 0;
            shootCount = 0;
            gameObject.GetComponent<SpriteRenderer>().color=new Color(0,0,0,0);
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    IEnumerator NotPass()
    {
        yield return new WaitForSeconds(2.5f);
        playUi.NotPassLevel();
    }
}
