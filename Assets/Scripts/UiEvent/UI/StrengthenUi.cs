using System;
using System.Collections;
using System.Collections.Generic;
using UiCore;
using UnityEngine;
using UnityEngine.UI;

public class StrengthenUi : View
{

    private Button backBtn;
    //飞机等级
    private Text planeRankText;

    private int rank;
    //技能
    private Transform skills;
    //升级道具
    private Transform strengthenContent;
    private GameObject onceItem;
    //升级材料字典<id,数量>
    //private Dictionary<string, int> dicStr;
    //道具设置
    private Image itemImg;
    private Text itemName;
    private Text itemValue;

    private Button strengthenBtn;
    private string[] allStrInfo;

    //提示
    private GameObject tipsWindow;
    private Text tipsText;

    protected override E_UIID uiid
    {
        get
        {
            return E_UIID.StrengthenUi;
        }
    }

    //public override string Name
    //{
    //    get
    //    {
    //        return "StrengthenUi";
    //    }
    //}

    protected override void InitDataOnAwake()
    {
        //uiid = E_UIID.StrengthenUi;
        uiType.ShowUiMode = E_ShowUIMode.HideOther;
        uiType.UiRootType = E_UiRootType.Normal;
        uiType.audio = E_Audio.Play;
        uiType.destroyType = E_DestroyType.ImmidiatelyDestroy;
    }

    protected override void InitUiOnAwake()
    {
        backBtn = GameTool.FindTheChild(gameObject, "BackBtn").GetComponent<Button>();
        planeRankText = GameTool.FindTheChild(gameObject, "PlaneRankText").GetComponent<Text>();
        skills = GameTool.FindTheChild(gameObject, "Skills").transform;
        strengthenContent = GameTool.FindTheChild(gameObject, "StrengthenContent").transform;
        onceItem= Resources.Load<GameObject>("UIPrefabs/OnceItem");
        strengthenBtn = GameTool.FindTheChild(gameObject, "StrengthenBtn").GetComponent<Button>();
        tipsWindow = GameTool.FindTheChild(gameObject, "TipsWindow").gameObject;
        tipsText = GameTool.FindTheChild(tipsWindow, "TipsText").GetComponent<Text>();

    }

    protected override void Start()
    {
        backBtn.onClick.AddListener(ToBackClick);
        strengthenBtn.onClick.AddListener(StrengthenBtnClick);

        planeRankText.text = PlayerManager.Instance.PlaneRank.ToString();

        
    }

    protected override void RegisterCtrl()
    {

    }

    protected override void OnEnable()
    {
        InitSkills();
        ClearContent(strengthenContent);
        InitStrengthen();
    }

    private void StrengthenBtnClick()
    {
        rank = PlayerManager.Instance.PlaneRank;
        //Debug.Log(rank);
        if (rank>=CfgController.Instance.dicPlane.Count)//超过最大等级
        {
            tipsWindow.SetActive(true);
            tipsText.text = "已到最大等级。";
            return;
        }

        string str = CfgController.Instance.ReadPlaneCfg(rank.ToString()).Strengthen;
        allStrInfo = str.Split(';');
        for (int i = 0; i < allStrInfo.Length; i++)
        {
            string[] strInfo = allStrInfo[i].Split(',');
            if (MVC.GetModel<PacketData>().ReadCountById(strInfo[0]) < int.Parse(strInfo[1]))//背包物品不够
            {
                tipsWindow.SetActive(true);
                tipsText.text = "物品数量不足，强化失败！";
                return;
            }
        }
        tipsWindow.SetActive(true);
        tipsText.text = "强化成功！";
        SuccessStrengthen();
    }

    /// <summary>
    /// 强化成功
    /// </summary>
    private void SuccessStrengthen()
    {
        for (int i = 0; i < allStrInfo.Length; i++)
        {
            string[] strInfo = allStrInfo[i].Split(',');
            //扣除背包物品
            int count = MVC.GetModel<PacketData>().ReadCountById(strInfo[0]);
            count -= int.Parse(strInfo[1]);
            MVC.GetModel<PacketData>().EditorItemCount(strInfo[0],count);
        }
        //升级与重置
        PlayerManager.Instance.PlaneRank += 1;
        InitSkills();
        ClearContent(strengthenContent);
        InitStrengthen();
        planeRankText.text = PlayerManager.Instance.PlaneRank.ToString();
    }

    /// <summary>
    /// 初始化技能
    /// </summary>
    private void InitSkills()
    {
        rank = PlayerManager.Instance.PlaneRank;
        int skill = CfgController.Instance.ReadPlaneCfg(rank.ToString()).Skill;
        //读取配置表的技能解锁度，显示解锁技能
        for (int i=0 ; i < skill; i++)
        {
            GameTool.FindTheChild(skills.GetChild(i).gameObject,"LockText").gameObject.SetActive(false);
            GameTool.FindTheChild(skills.GetChild(i).gameObject, "LockImg").gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 初始化强化材料
    /// </summary>
    private void InitStrengthen()
    {
        //dicStr = new Dictionary<string, int>();
        rank = PlayerManager.Instance.PlaneRank;
        string str = CfgController.Instance.ReadPlaneCfg(rank.ToString()).Strengthen;
        allStrInfo = str.Split(';');
        for (int i = 0; i < allStrInfo.Length; i++)
        {
            string[] strInfo = allStrInfo[i].Split(',');
            //dicStr.Add(enemyInfor[0], int.Parse(enemyInfor[1]));

            GameObject goods = Instantiate(onceItem);
            GameTool.AddChildToParent(strengthenContent,goods.transform);
            goods.AddComponent<ItemEneity>().id = strInfo[0];

            string iconName = CfgController.Instance.ReadItemCfg(strInfo[0].ToString()).IconName;
            itemImg = GameTool.FindTheChild(goods, "ItemImg").GetComponent<Image>();
            itemImg.sprite= Resources.Load<Sprite>("GamePrefabs/Icon/" + iconName);

            itemValue = GameTool.FindTheChild(goods, "ItemValue").GetComponent<Text>();
            itemValue.text = strInfo[1];

            string name= CfgController.Instance.ReadItemCfg(strInfo[0].ToString()).Name;
            itemName = GameTool.FindTheChild(goods, "ItemName").GetComponent<Text>();
            itemName.text = name;
        }
        strengthenContent.GetComponent<RectTransform>().sizeDelta=new Vector2(240*strengthenContent.childCount,220);
    }

    /// <summary>
    /// 清空强化物品栏
    /// </summary>
    /// <param name="content"></param>
    private void ClearContent(Transform content)
    {
        if (content.childCount > 0)
        {
            for (int i = content.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(content.GetChild(i).gameObject);
            }
        }
    }

    public override void AddMessageListener()
    {
        
    }

    public override void RemoveMessageListener()
    {
        
    }

    //public override void HandleEvent(string eventName, object data)
    //{
    //    throw new NotImplementedException();
    //}
}
