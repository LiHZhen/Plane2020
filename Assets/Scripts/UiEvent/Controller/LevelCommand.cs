using System.Collections;
using System.Collections.Generic;
using GameCore;
using UiCore;
using UnityEngine;
using UnityEngine.UI;

public class LevelCommand : Controller
{
    private LevelUi levelUi;

    public override void Execute(object data1,object data2)
    {
        if (data1 == null)
        {
            levelUi = (LevelUi)data2;
            ShowLevelOnEnable();
        }
        else
        {
            levelUi = UiManager.Instance.GetUiScript<LevelUi>();
            E_LevelCmdType type = (E_LevelCmdType)data1;
            switch (type)
            {
                case E_LevelCmdType.PlayGame:
                    PlayGame();
                    break;


                default:
                    break;
            }
        }

        

        
    }

    private void PlayGame()
    {
        if (LevelManager.Instance.GetCurrentEnterLevel() == 0)
        {
            return;
        }
        SceneController.Instance.LoadSceneAsync("SceneLevel1", () =>
        {
            //lambda表达式
            //把方法带去LoadingUi，让进度条加载完再执行
            UiManager.Instance.ShowUI(E_UIID.PlayUi, false);

        });
    }

    private void ShowLevelOnEnable()
    {
        levelUi.myPrice.text = MVC.GetModel<PacketData>().ReadCountById("1").ToString();
        if (levelUi.levelContent.transform.childCount != 0)
        {
            for (int i = levelUi.levelContent.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(levelUi.levelContent.transform.GetChild(i).gameObject);
            }
        }
        LoadAllLevel();
    }

    /// <summary>
    /// 加载配置表内所有光卡
    /// </summary>
    private void LoadAllLevel()
    {
        int levelCount = CfgController.Instance.dicLevel.Count;
        //加载所有关卡
        for (int i = 0; i < levelCount; i++)
        {
            GameObject level = Instantiate(levelUi.levelBtn);
            level.AddComponent<LevelEntity>().levelId = i + 1;
            GameTool.AddChildToParent(levelUi.levelContent.transform, level.transform);
            //设置关卡数值
            GameTool.FindTheChild(level, "LevelText").GetComponent<Text>().text = (i + 1).ToString();
            //根据关卡数量设置Content高度
            levelUi.levelContent.GetComponent<RectTransform>().sizeDelta = new Vector2(levelUi.levelContent.GetComponent<RectTransform>().sizeDelta.x, 25 + (132 * (levelCount / 3)));
        }
        //设置未开启的关卡
        for (int i = levelUi.levelContent.transform.childCount - 1; i >= LevelManager.Instance.GetCurrentMaxLevel(); i--)
        {
            GameObject level = levelUi.levelContent.transform.GetChild(i).gameObject;
            GameObject block = GameTool.FindTheChild(level, "BlockImg").gameObject;
            level.GetComponent<Button>().enabled = false;
            block.SetActive(true);

        }
        //读取并设置关卡星数
        Transform levelTrans;
        for (int i = 0; i < levelUi.levelContent.transform.childCount; i++)
        {
            levelTrans = levelUi.levelContent.transform.GetChild(i);
            ShowLevelStar(i, levelTrans);
        }
    }


    private void ShowLevelStar(int levelId, Transform levelTrans)
    {
        //根据关卡的Id读取星级数
        int starCount = LevelManager.Instance.GetStarCount(levelId + 1);
        //if (levelId == 0)
        //{
        //    Debug.Log(starCount);
        //}
        Sprite sp = GetStarSprite(starCount);
        if (sp != null)
        {
            GameObject starImgGO = GameTool.FindTheChild(levelTrans.gameObject, "StarImg").gameObject;
            starImgGO.GetComponent<Image>().sprite = sp;
            //Debug.Log(levelId+","+starCount);
        }
    }

    private Sprite GetStarSprite(int count)
    {
        Sprite sp;
        switch (count)
        {
            case 1:
                sp = levelUi.spriteStar1;
                break;
            case 2:
                sp = levelUi.spriteStar2;
                break;
            case 3:
                sp = levelUi.spriteStar3;
                break;
            default:
                sp = null; ;
                break;
        }
        return sp;
    }
}
