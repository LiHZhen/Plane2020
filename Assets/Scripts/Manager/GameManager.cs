using System.Collections;
using System.Collections.Generic;
using UiCore;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private GameObject playerPrefab;

    private Text scoreText;
    //计分
    private float gameScore = 0f;
    public float GameScore
    {
        get
        {
            return gameScore;
        }

        set
        {
            gameScore = value;
            scoreText.text = "分数:" + gameScore;
        }
    }

    public bool makeEnemysOver = false;

    private Transform enemyItem;

    private int passScore1 = 0;
    private int passScore2 = 0;
    private int passScore3 = 0;

    private PlayUi playUi=null;

    //储存关卡奖励的字典<id,数量>
    private Dictionary<string, int> dicReward = new Dictionary<string, int>(); 
    //显示奖励的列表
    private Transform rewardContent;
    //物品
    private GameObject oneReward;
    //物品图片组件
    private Image rewardImg;
    //要加载的物品图片
    private Sprite rewardSprite;
    //数量显示
    private Text rewardText;

    

    //单例模式
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Find("GameManager").GetComponent<GameManager>();
                instance.scoreText = GameObject.Find("PlayUi(Clone)/Score").GetComponent<Text>();
            }
            return instance;
        }
    }

    

    void Awake()
    {
        playerPrefab = Resources.Load<GameObject>("GamePrefabs/PlayerShip");
        enemyItem = GameObject.Find("EnemyItem").transform;

        
    }

	void Start ()
    {
        //实例化对象池
        gameObject.AddComponent<ObjectPool>();
        //实例化主角
        GameObject player = Instantiate(playerPrefab,new Vector3(0,-1,0),Quaternion.identity);
        player.AddComponent<PlayerShipMove>();
        //实例化敌人管理器
        gameObject.AddComponent<EnemyManager>();

        //把关卡奖励配置表转换为字典
        string rewordData = CfgController.Instance.ReadLevelCfg(LevelManager.Instance.GetCurrentEnterLevel().ToString()).Reward;
        string[] allRewordInfo = rewordData.Split(';');
        for (int i = 0; i < allRewordInfo.Length; i++)
        {
            string[] rewordInfo = allRewordInfo[i].Split(',');
            dicReward.Add(rewordInfo[0], int.Parse(rewordInfo[1]));
        }
    }
	
	// Update is called once per frame
	void Update () {
        //所有敌机创建完成
        if (makeEnemysOver)
        {
            if (enemyItem.childCount == 0)
            {
                StartCoroutine(GameEnd());
                makeEnemysOver = false;
            }
        }
    }
    IEnumerator GameEnd()//游戏通关
    {
        yield return new WaitForSeconds(2f);
        passScore1 = int.Parse(CfgController.Instance.ReadLevelCfg(LevelManager.Instance.GetCurrentEnterLevel().ToString()).OneStarPass);
        passScore2 = int.Parse(CfgController.Instance.ReadLevelCfg(LevelManager.Instance.GetCurrentEnterLevel().ToString()).TwoStarPass);
        passScore3 = int.Parse(CfgController.Instance.ReadLevelCfg(LevelManager.Instance.GetCurrentEnterLevel().ToString()).ThirdStarPass);
        playUi=GameObject.Find("PlayUi(Clone)").GetComponent<PlayUi>();
        if (GameScore >= passScore3)
        {
            playUi.PassLevel(3);
        }
        else if (GameScore >= passScore2)
        {
            playUi.PassLevel(2);
        }
        else if(GameScore>=passScore1)
        {
            playUi.PassLevel(1);
        }
        else
        {
            playUi.PassLevel(0);
        }
    }

    public void ShowAllReward()
    {
        //for (int i = 0; i < dicReward.Count; i++)
        //{
        //    GameObject item = Instantiate(oneReward);
        //    item.transform.SetParent(rewardContent);
        //    rewardImg = GameTool.FindTheChild(item, "Image").GetComponent<Image>();
        //    string iconName = CfgController.Instance.ReadItemCfg(dicReward[i]).IconName;
        //    rewardSprite = Resources.Load<Sprite>("GamePrefabs/Icon/"+name);
        //}
        rewardContent =GameTool.FindTheChild(playUi.gameObject, "GiftContent").transform;
        oneReward = Resources.Load<GameObject>("UIPrefabs/LevelPassGift");
        foreach (KeyValuePair<string,int> value in dicReward)
        {
            //增加背包物品
            int count = MVC.GetModel<PacketData>().ReadCountById(value.Key);
            //Debug.Log(value.Value);
            MVC.GetModel<PacketData>().EditorItemCount(value.Key,count+value.Value);
            //设置物品显示
            GameObject item = Instantiate(oneReward);
            //item.transform.SetParent(rewardContent);
            GameTool.AddChildToParent(rewardContent,item.transform);
            rewardImg = GameTool.FindTheChild(item, "Image").GetComponent<Image>();
            rewardText= GameTool.FindTheChild(item, "Text").GetComponent<Text>();
            string iconName = CfgController.Instance.ReadItemCfg(value.Key).IconName;
            rewardSprite = Resources.Load<Sprite>("GamePrefabs/Icon/" + iconName);
            rewardText.text = value.Value.ToString();
            //Debug.Log(iconName);
            rewardImg.sprite = rewardSprite;
        }
    }
}
