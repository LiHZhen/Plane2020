using GameCore;
using System.Collections;
using System.Collections.Generic;
using UiCore;
using UnityEngine;
using UnityEngine.UI;

public class LevelSweepCommand : Controller
{
    private LevelUi levelUi;

    public override void Execute(object data1, object data2)
    {
        levelUi = UiManager.Instance.GetUiScript<LevelUi>();
        if (data2 != null|| UiManager.Instance.GetUiScript<LevelUi>()==null)
        {
            levelUi = (LevelUi)data2;
        }
        
        E_LevelCmdType type = (E_LevelCmdType)data1;
        switch (type)
        {
            case E_LevelCmdType.SweepBtnClick:
                SweepBtnClick();
                break;
            case E_LevelCmdType.InitSweep:
                InitSweepCount();
                break;
            case E_LevelCmdType.SweepEnter:
                SweepEnter();
                break;
            case E_LevelCmdType.ShowPassGive:
                ShowPassGift();
                break;

            default:
                break;
        }
           
    }


    /// <summary>
    /// 开始扫荡
    /// </summary>
    private void SweepEnter()
    {
        if (levelUi.sweepCount <= 0)//是否还有次数
        {
            levelUi.tipsText.text = "剩余次数为0，无法扫荡。";
            levelUi.gameObject.SetActive(true);

            return;
        }
        //判断是否3星
        if (LevelManager.Instance.GetStarCount(LevelManager.Instance.GetCurrentEnterLevel()) < 3)
        {
            levelUi.tipsText.text = "未取得3星，无法扫荡。";
            levelUi.tipsPanel.SetActive(true);
            return;
        }
        levelUi.level = LevelManager.Instance.GetCurrentEnterLevel();
        if (levelUi.level > 0)
        {
            int price = CfgController.Instance.ReadLevelCfg(levelUi.level.ToString()).SweepPrice;
            int myCoin = MVC.GetModel<PacketData>().ReadCountById("1");
            myCoin -= price;
            MVC.GetModel<PacketData>().EditorItemCount("1", myCoin);
            //把关卡奖励配置表转换为字典
            string rewordData = CfgController.Instance.ReadLevelCfg(LevelManager.Instance.GetCurrentEnterLevel().ToString()).Reward;
            string[] allRewordInfo = rewordData.Split(';');
            //for (int i = 0; i < allRewordInfo.Length; i++)
            //{
            //    string[] rewordInfo = allRewordInfo[i].Split(',');
            //    dicReward.Add(rewordInfo[0], int.Parse(rewordInfo[1]));
            //}
            foreach (KeyValuePair<string, int> value in StringToDic(rewordData))
            {
                //增加背包物品
                int count = MVC.GetModel<PacketData>().ReadCountById(value.Key);
                MVC.GetModel<PacketData>().EditorItemCount(value.Key, count + value.Value);
            }

            ResetSweepCount();
            InitSweepCount();
            levelUi.dicReward.Clear();

            SceneController.Instance.LoadSceneAsync("Main", () =>
            {
                UiManager.Instance.ShowUI(E_UIID.LevelUi, false);
            });
        }

    }

    /// <summary>
    /// 把分隔开的string类型奖励列表转换为字典
    /// </summary>
    /// <param name="str">分隔开的字符串</param>
    /// <returns>奖励字典<id,数量></returns>
    public Dictionary<string, int> StringToDic(string rewordData)
    {
        levelUi.dicReward.Clear();
        string[] allRewordInfo = rewordData.Split(';');
        for (int i = 0; i < allRewordInfo.Length; i++)
        {
            string[] rewordInfo = allRewordInfo[i].Split(',');
            levelUi.dicReward.Add(rewordInfo[0], int.Parse(rewordInfo[1]));
        }
        return levelUi.dicReward;
    }

    public void SweepBtnClick()
    {
        levelUi.level = LevelManager.Instance.GetCurrentEnterLevel();
        if (levelUi.level == 0)
        {
            levelUi.sweepTipsText.text = "未选择关卡。";
            levelUi.enterBtn.enabled = false;
            return;
        }
        else
        {
            levelUi.sweepTipsText.text = "扫荡需要花费";
            levelUi.sweepPrice.text = CfgController.Instance.ReadLevelCfg(levelUi.level.ToString()).SweepPrice.ToString();
            levelUi.enterBtn.enabled = true;
        }
    }
    /// <summary>
    /// 初始化扫荡次数
    /// </summary>
    public void InitSweepCount()
    {
        string today = System.DateTime.Now.Year + "/" + System.DateTime.Now.Month + "/" + System.DateTime.Now.Day;

        if (PlayerPrefs.GetString("Today") == "")//第一次运行游戏
        {
            PlayerPrefs.SetString("Today", today);
            //MVC.GetModel<SweepData>().ResetValue();
            //Debug.Log("第一次");
        }
        else
        {
            string before = PlayerPrefs.GetString("Today");
            if (!before.Equals(today))
            {
                //不是今天
                PlayerPrefs.SetString("Today", today);
                MVC.GetModel<SweepData>().ResetValue();
            }
            else
            {

            }
        }

        levelUi.level = LevelManager.Instance.GetCurrentEnterLevel();

        if (MVC.GetModel<SweepData>().ReadCountByKey(levelUi.level.ToString()) == -1)
        {
            MVC.GetModel<SweepData>().EditCountByKey(levelUi.level.ToString(), 3);
        }
        levelUi.sweepCount = MVC.GetModel<SweepData>().ReadCountByKey(levelUi.level.ToString());
        //Debug.Log(MVC.GetModel<SweepData>().dicSweep[level.ToString()]);
        levelUi.sweepTimesOneDay.text = levelUi.sweepCount.ToString();
    }

    /// <summary>
    ///  扫荡成功后
    /// </summary>
    private void ResetSweepCount()
    {
        levelUi.level = LevelManager.Instance.GetCurrentEnterLevel();
        MVC.GetModel<SweepData>().AddCountByKey(levelUi.level.ToString(), -1);
    }

    /// <summary>
    /// 显示通关奖励
    /// </summary>
    public void ShowPassGift()
    {
        ClearContent(levelUi.giftContent.transform);
        levelUi.level = LevelManager.Instance.GetCurrentEnterLevel();
        string gift = CfgController.Instance.ReadLevelCfg(levelUi.level.ToString()).Reward;
        GameObject onceGift;
        foreach (KeyValuePair<string, int> value in StringToDic(gift))
        {
            onceGift = Instantiate(levelUi.onceGift);
            GameTool.AddChildToParent(levelUi.giftContent, onceGift.transform);
            levelUi.giftNameText = onceGift.transform.GetChild(0).gameObject.GetComponent<Text>();
            levelUi.giftNameText.text = CfgController.Instance.ReadItemCfg(value.Key).Name;
            levelUi.giftCountText = onceGift.transform.GetChild(1).gameObject.GetComponent<Text>();
            levelUi.giftCountText.text = value.Value.ToString();
        }
    }

    /// <summary>
    /// 清空物品
    /// </summary>
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
}
