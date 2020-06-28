using System;
using System.Collections;
using System.Collections.Generic;
using UiCore;
using UnityEngine;
using UnityEngine.UI;

public class PacketUi : View
{

    private Button backBtn;
    //金币数量
    private Text cointText;
    //物品列表
    private Transform packetContent;
    private GameObject onceGoodItem;

    //物品筛选
    private ToggleGroup packetTog;
    private Toggle allTog;
    private Toggle gameTog;
    private Toggle expTog;
    private Toggle strTog;

    protected override E_UIID uiid
    {
        get { return E_UIID.LevelUi; }
    }

    //public override string Name
    //{
    //    get
    //    {
    //        return "PacketUi";
    //    }
    //}

    protected override void InitDataOnAwake()
    {
        //uiid = E_UIID.PacketUi;
        uiType.ShowUiMode = E_ShowUIMode.HideOther;
        uiType.UiRootType = E_UiRootType.Normal;
        uiType.audio = E_Audio.Play;
        uiType.destroyType = E_DestroyType.ImmidiatelyDestroy;
    }

    protected override void InitUiOnAwake()
    {
        base.InitUiOnAwake();
        
        backBtn = GameTool.FindTheChild(gameObject, "BackBtn").GetComponent<Button>();
        cointText = GameTool.FindTheChild(gameObject, "CoinsText").GetComponent<Text>();
        packetContent = GameTool.FindTheChild(gameObject, "PacketContent").transform;
        onceGoodItem = Resources.Load<GameObject>("UIPrefabs/OnceItem");

        packetTog = GameTool.FindTheChild(gameObject, "PacketToggles").GetComponent<ToggleGroup>();
        allTog = packetTog.transform.GetChild(0).GetComponent<Toggle>();
        gameTog = packetTog.transform.GetChild(1).GetComponent<Toggle>();
        expTog = packetTog.transform.GetChild(2).GetComponent<Toggle>();
        strTog = packetTog.transform.GetChild(3).GetComponent<Toggle>();

        int i = GetModel<PacketData>().PacketModelTest(1);
        SendCommand(E_CommandType.InitPacketCmd,1);
    }

    protected override void OnEnable()
    {
        ClearPacket();
        InitPacket();
        allTog.isOn = true;
    }

    protected override void Start()
    {
        backBtn.onClick.AddListener(ToBackClick);
        
        allTog.onValueChanged.AddListener(OnTogValueCange);
        gameTog.onValueChanged.AddListener(OnTogValueCange);
        expTog.onValueChanged.AddListener(OnTogValueCange);
        strTog.onValueChanged.AddListener(OnTogValueCange);
    }

    private void OnTogValueCange(bool isOn)
    {
        if (allTog.isOn)
        {
            SetPacketShowType(0);//所有道具
        }
        else if (gameTog.isOn)
        {
            SetPacketShowType(1);//游戏道具
        }
        else if (expTog.isOn)
        {
            SetPacketShowType(3);//经验道具
        }
        else if (strTog.isOn)
        {
            SetPacketShowType(2);//强化道具
        }
    }

    private void SetPacketShowType(int type)
    {
        for (int i = 0; i < packetContent.childCount; i++)
        {
            if (type == 0)
            {
                packetContent.GetChild(i).gameObject.SetActive(true);
            }
            else if (packetContent.GetChild(i).GetComponent<ItemEneity>().type != type)
            {
                packetContent.GetChild(i).gameObject.SetActive(false);
            }
            else
            {
                packetContent.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// 清空背包物品
    /// </summary>
    private void ClearPacket()
    {
        if (packetContent.childCount > 0)
        {
            for (int i = packetContent.childCount-1; i >= 0; i--)
            {
                DestroyImmediate(packetContent.GetChild(i).gameObject);
            }
        }
    }

    /// <summary>
    /// 初始化背包物品
    /// </summary>
    private void InitPacket()
    {
        cointText.text = MVC.GetModel<PacketData>().ReadCountById("1").ToString();
        //id:1为金币
        if (MVC.GetModel<PacketData>().GetGoodsCount() > 1)
        {
            for (int i = 2; i <= MVC.GetModel<PacketData>().GetGoodsCount(); i++)
            {
                if (MVC.GetModel<PacketData>().ReadCountById(i.ToString()) == 0)
                { 
                    continue;
                }
                GameObject goods = Instantiate(onceGoodItem);
                GameTool.AddChildToParent(packetContent, goods.transform);
                goods.AddComponent<ItemEneity>().id = i.ToString();
                goods.GetComponent<ItemEneity>().type = CfgController.Instance.ReadItemCfg(i.ToString()).Type;
                SetGoodsEneity(goods, i);
            }

            int y = (MVC.GetModel<PacketData>().GetGoodsCount() - 1) / 4;
            packetContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 250 * y);
        }
    }

    /// <summary>
    /// 设置背包物品的图片，数量和显示类别
    /// </summary>
    /// <param name="goods"></param>
    private void SetGoodsEneity(GameObject goods, int id)
    {
        
        string iconName = CfgController.Instance.ReadItemCfg(id.ToString()).IconName;
        Image img = GameTool.FindTheChild(goods, "ItemImg").GetComponent<Image>();
        img.sprite = Resources.Load<Sprite>("GamePrefabs/Icon/" + iconName);
        //Debug.Log(img.sprite);

        int count = MVC.GetModel<PacketData>().ReadCountById(id.ToString());
        GameTool.FindTheChild(goods, "ItemValue").GetComponent<Text>().text = count.ToString();

        string name= CfgController.Instance.ReadItemCfg(id.ToString()).Name;
        GameTool.FindTheChild(goods, "ItemName").GetComponent<Text>().text = name;
    }

    public override void AddMessageListener()
    {
        
    }

    public override void RemoveMessageListener()
    {
        
    }

   
    protected override void RegisterCtrl()
    {
        MVC.RegisterController(E_CommandType.InitPacketCmd,typeof(PacketCommand));
    }

    //public override void HandleEvent(string eventName, object data)
    //{
    //    throw new NotImplementedException();
    //}
}
