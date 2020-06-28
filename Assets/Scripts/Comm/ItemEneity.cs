using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.UI;

public class ItemEneity : MonoBehaviour
{

    public string id = "0";

    public Button goodsBtn;

    public int type = 0;

    private void Awake()
    {
        goodsBtn = GameTool.FindTheChild(gameObject,"ItemBg").GetComponent<Button>();
    }

    private void Start()
    {
        goodsBtn.onClick.AddListener(delegate() { UiManager.Instance.SelectId = id; });
    }
}
