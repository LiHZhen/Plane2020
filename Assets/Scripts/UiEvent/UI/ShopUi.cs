using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UiCore;
using UnityEngine;
using UnityEngine.UI;

public class ShopUi : View
{

    private Button backBtn;

    //金币数量
    private Text cointText;
    //物品列表
    private Transform shopContent;
    private GameObject onceGoodItem;

    private GameObject tipsWindow;
    private Button addBtn;
    private Button subBtn;
    private Button buyBtn;
    private InputField buyCountText;
    private Text tipsText;
    private Button closeTipsBtn;

    protected override E_UIID uiid
    {
        get { return E_UIID.ShopUi; }
    }

    //public override string Name
    //{
    //    get
    //    {
    //        return "ShopUi";
    //    }
    //}

    protected override void InitDataOnAwake()
    {
        //uiid = E_UIID.ShopUi;
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
        shopContent = GameTool.FindTheChild(gameObject, "ShopContent").transform;
        onceGoodItem = Resources.Load<GameObject>("UIPrefabs/OnceItem");

        tipsWindow = GameTool.FindTheChild(gameObject, "TipsWindow").gameObject;
        addBtn = GameTool.FindTheChild(gameObject, "AddBtn").GetComponent<Button>();
        subBtn = GameTool.FindTheChild(gameObject, "SubBtn").GetComponent<Button>();
        buyBtn = GameTool.FindTheChild(gameObject, "BuyBtn").GetComponent<Button>();
        buyCountText = GameTool.FindTheChild(gameObject, "BuyInput").GetComponent<InputField>();

        tipsText = GameTool.FindTheChild(tipsWindow, "TipsText").GetComponent<Text>();
        closeTipsBtn = GameTool.FindTheChild(tipsWindow, "CloseBtn").GetComponent<Button>();

        
    }

    protected override void Start()
    {
        backBtn.onClick.AddListener(ToBackClick);
        addBtn.onClick.AddListener(delegate() { EditBuyCount(1); });
        subBtn.onClick.AddListener(delegate() { EditBuyCount(-1); });
        buyBtn.onClick.AddListener(BuyEnter);

        closeTipsBtn.onClick.AddListener(CloseTips);
    }
    protected override void RegisterCtrl()
    {

    }
    private void CloseTips()
    {
        tipsWindow.SetActive(false);
    }

    protected override void OnEnable()
    {
        ClearShop();
        InitShop();
        ChooseGoods("0");
    }

    /// <summary>
    /// 初始化背包物品
    /// </summary>
    private void InitShop()
    {
        cointText.text = MVC.GetModel<PacketData>().ReadCountById("1").ToString();

        for (int i = 2; i <= CfgController.Instance.dicItem.Count; i++)
        {
            GameObject goods = Instantiate(onceGoodItem);
            GameTool.AddChildToParent(shopContent, goods.transform);
            goods.AddComponent<ItemEneity>().id = i.ToString();
            SetGoodsEneity(goods, i);
        }
        int y = (CfgController.Instance.dicItem.Count-1) / 4;
        shopContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 250 * y);
    }

    /// <summary>
    /// 设置背包物品的图片，数量和显示类别
    /// </summary>
    /// <param name="goods"></param>
    /// <param name="i"></param>
    private void SetGoodsEneity(GameObject goods, int id)
    {
        string iconName = CfgController.Instance.ReadItemCfg(id.ToString()).IconName;
        Image img = GameTool.FindTheChild(goods, "ItemImg").GetComponent<Image>();
        img.sprite = Resources.Load<Sprite>("GamePrefabs/Icon/" + iconName);

        int price = CfgController.Instance.ReadItemCfg(id.ToString()).Price;
        GameTool.FindTheChild(goods, "ItemValue").GetComponent<Text>().text = price.ToString();

        string name = CfgController.Instance.ReadItemCfg(id.ToString()).Name;
        GameTool.FindTheChild(goods, "ItemName").GetComponent<Text>().text = name;
    }

    /// <summary>
    /// 清空背包物品
    /// </summary>
    private void ClearShop()
    {
        if (shopContent.childCount == 0)
        {
            for (int i = 0; i < shopContent.childCount; i++)
            {
                DestroyImmediate(shopContent.GetChild(i).gameObject);
            }
        }
    }

    /// <summary>
    /// 加减编辑购买数量
    /// </summary>
    /// <param name="value"></param>
    private void EditBuyCount(int value)
    {
        
        int count = int.Parse(buyCountText.text)+value;
        if (count >= 9999)
        {
            count-=value;
        }
        buyCountText.text = count.ToString();
    }

    /// <summary>
    /// 选择商品
    /// </summary>
    /// <param name="id"></param>
    public void ChooseGoods(string id)
    {
        UiManager.Instance.SelectId = id;
    }

    /// <summary>
    /// 点击购买
    /// </summary>
    private void BuyEnter()
    {
        int count = int.Parse(buyCountText.text);
        int price = 0;
        string tips="";
        if (!UiManager.Instance.SelectId.Equals("0"))
        {
            price = CfgController.Instance.ReadItemCfg(UiManager.Instance.SelectId).Price * count;
            if (count <= 0)
            {
                tips = "请选择数量。";
            }

            else if (price > MVC.GetModel<PacketData>().ReadCountById("1"))
            {
                tips = "金币不足！";
            }
            else
            {
                tips = "购买成功！";

                //设置背包物品
                int itemCount = MVC.GetModel<PacketData>().ReadCountById(UiManager.Instance.SelectId);
                MVC.GetModel<PacketData>().EditorItemCount(UiManager.Instance.SelectId, count + itemCount);
                //设置金币
                int coins = MVC.GetModel<PacketData>().ReadCountById("1");
                MVC.GetModel<PacketData>().EditorItemCount("1", coins - price);
                cointText.text=MVC.GetModel<PacketData>().ReadCountById("1").ToString();
            }
            
        }
        else
        {
            tips = "未选择商品。";
        }
        

        buyCountText.text = "0";
        tipsText.text = tips;
        tipsWindow.SetActive(true);
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
