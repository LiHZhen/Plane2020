using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using UiCore;

public class GoodsModel
{
    //序列化与反序列化字典，字典的键只能用string类型
    public Dictionary<string, int> dicGoods = new Dictionary<string, int>();
}

public class PacketData : Model {

    GoodsModel goodsModel = new GoodsModel();

    public override string Name
    {
        get { return "PacketData"; }
    }

    public override string Path
    {
        get
        {
            return Application.persistentDataPath + "\\PackData.json";
        }
    }


    //初始化背包数据的方法
    //public override void InitData()
    //{
    //    if (!File.Exists(Path))
    //    {
    //        goodsModel.dicGoods.Add("1",99999999);
    //        SaveData(goodsModel);
    //    }
    //    else
    //    {
    //        CoroutineController.Instance.StartCoroutine(LoadData());
    //    }

    //}

    //private void SaveData()
    //{
    //    string jsonData = JsonMapper.ToJson(goodsModel);
    //    using (StreamWriter sw = new StreamWriter(Path))
    //    {
    //        sw.WriteLine(jsonData);
    //    }
    //}

    //IEnumerator ReadData()
    //{
    //    WWW www = new WWW("file:///" + Path);
    //    yield return www;
    //    string json = www.text;
    //    goodsModel = JsonMapper.ToObject<GoodsModel>(json);
    //}

    /// <summary>
    /// //对外提供的，根据物品ID获取物品数量
    /// </summary>
    /// <param name="itemId">id</param>
    /// <returns></returns>
    public int ReadCountById(string itemId)
    {
        if (goodsModel.dicGoods.ContainsKey(itemId.ToString()))
        {
            return goodsModel.dicGoods[itemId.ToString()];
        }
        else
        {
            goodsModel.dicGoods.Add(itemId,0);
            SaveData(goodsModel);
        }
        return 0;
    }

    /// <summary>
    /// 对外提供的，根据物品ID设置物品数量
    /// </summary>
    /// <param name="itemId"></param>
    /// <param name="newCount"></param>
    public void EditorItemCount(string itemId, int newCount)
    {
        //Debug.Log(itemId);
        //Debug.Log(newCount);
        if (goodsModel.dicGoods.ContainsKey(itemId.ToString()))
        {
            goodsModel.dicGoods[itemId.ToString()] = newCount;
        }
        else
        {
            goodsModel.dicGoods.Add(itemId, newCount);
        }
        SaveData(goodsModel);
    }

    /// <summary>
    /// 返回拥有的物品种类个数
    /// </summary>
    /// <returns></returns>
    public int GetGoodsCount()
    {
        return goodsModel.dicGoods.Count;
    }

    public int PacketModelTest(int i)
    {
        Debug.Log("PacketDataModel"+i);
        SendMessage(E_MessageType.InitPacket,i);
        return i;
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    protected override void JsonToObj()
    {
        goodsModel = JsonMapper.ToObject<GoodsModel>(json);
    }
}
