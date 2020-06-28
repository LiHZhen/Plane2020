using System;
using System.Collections;
using System.Collections.Generic;
using UiCore;
using UnityEngine;


namespace GameCore
{
    //UI管理类
    public class UiManager : UnitySingleton<UiManager>
    {
        //缓存打开过的窗体脚本
        private Dictionary<E_UIID, View> dicAllUi; 
        //缓存正在显示的窗体
        private Dictionary<E_UIID, View> dicShowUi;
        //缓存上一个窗体的ID
        private E_UIID beforeUIID = E_UIID.NullUi;
        //缓存画布
        private Transform canvas;
        //缓存画布下方的两个窗体关节点
        private Transform keepAboveUIRot;
        private Transform normalUIRoot;

        private bool firstTimeAwake = true;//初次实例化

        private float logoTime = 3;

        private string selectId = "0";
        public string SelectId
        {
            get
            {
                return selectId;
            }

            set
            {
                selectId = value;
            }
        }


        void Awake()
        {
            if (firstTimeAwake)
            {
                if (dicAllUi == null)
                {
                    dicAllUi = new Dictionary<E_UIID, View>();
                }
                if (dicShowUi == null)
                {
                    dicShowUi = new Dictionary<E_UIID, View>();
                }
                InitUIManager();
                firstTimeAwake = false;
            }
        }

        /// <summary>
        /// 根据ID获取窗体Obj
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GameObject GetUiObj(E_UIID id)
        {
            if (dicAllUi.ContainsKey(id))
            {
                return dicAllUi[id].gameObject;
            }

            return null;
        }

        /// <summary>
        /// 获取窗体上面挂载的UI脚本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetUiScript<T>() where T : View, new()
        {
            T t = new T();
            foreach (View item in dicAllUi.Values)
            {
                if (item.Uiid == t.Uiid)
                {
                    return item.GetComponent<T>();
                }
            }

            return null;
        }

        private void InitUIManager()//初始化窗体
        {
            canvas = this.transform.parent;
            //切换场景时画布不会被销毁
            DontDestroyOnLoad(canvas);
            if (keepAboveUIRot == null)
            {
                keepAboveUIRot = GameTool.FindTheChild(canvas.gameObject, "KeepAboveUIRot");
            }
            if (normalUIRoot == null)
            {
                normalUIRoot = GameTool.FindTheChild(canvas.gameObject, "NormalUIRoot");
            }
            ShowUI(E_UIID.LogoUi);
            StartCoroutine(StartGame());
        }

        IEnumerator StartGame()
        {
            yield return new WaitForSeconds(logoTime);
            HideSingleUI(E_UIID.LogoUi);
            ShowUI(E_UIID.MainUi);
            //ShowUI(E_UIID.InfoUi);
        }

        public void HideSingleUI(E_UIID uiID)//供外界调用的隐藏窗体方法
        {
            if (dicShowUi.ContainsKey(uiID))
            {
                dicShowUi[uiID].HideUI();
                dicShowUi.Remove(uiID);
            }
        }

        public void ShowUI(E_UIID uiid,bool isSaveBeforeID=true)//显示窗体
        {
            if (uiid == E_UIID.NullUi)
            {
                //Debug.Log("窗体ID为NullUi");
                //return;
                uiid = E_UIID.MainUi;
            }
            View View = JudgeShowUI(uiid);
            if (isSaveBeforeID)
            {
                View.BeforeUiid = beforeUIID;
            }
        }

        private View JudgeShowUI(E_UIID uiid) //判断要显示的是哪个UI,返回现在显示的UI
        {
            //判断要显示的窗体是否正在显示，如果正在显示则不用处理
            if (dicShowUi.ContainsKey(uiid))
            {
                
                return null;
                
            }
            View View=GetGaseUI(uiid);
            if (View == null)//判断需要显示的窗体是否加载过
            {
                //拿到UIID然后向UI路径字典取值
               string path=GameDefine.dicPath[uiid];
               GameObject loadUi= Resources.Load<GameObject>(path);
               if (loadUi != null)
               {
                   //路径正确，实例化窗体
                   GameObject willShowUI=Instantiate(loadUi);
                   View=willShowUI.GetComponent<View>();
                   Type type = GameDefine.GetUiScriptType(uiid);
                   if (View == null)
                   {
                       //说明窗体是没有挂载脚本，自动添加对应的UI脚本
                       View=willShowUI.AddComponent(type)as View;
                    }
                   else
                   {
                       View = willShowUI.GetComponent(type) as View;
                    }
                   Transform uiRoot = GetTheRoot(View);
                   GameTool.AddChildToParent(uiRoot, willShowUI.transform);
                   //位置不正确，重设UI位置
                   willShowUI.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
                   willShowUI.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;

                    //窗体第一次加载，把该窗体缓存到aicAllUi字典。
                    dicAllUi.Add(uiid,View);
                }
                else
               {
                   Debug.Log("没有加载到"+uiid+"预制体");
               }
            }
            else
            {
                View.ShowUI();
            }
            UpdateShowUiHedeUi(View);
            return View;
        }

        private void UpdateShowUiHedeUi(View View)//更新正在显示的字典并隐藏上一个显示的窗体
        {
            //判断窗体的类型是否需要隐藏其他窗体
            if (View.IsNeedDealWithUi)
            {
                //判断当前是否有正在显示的窗体,第一次加载时为空
                if (dicShowUi.Count > 0)
                {
                    //隐藏所有窗体
                    if (View.uiType.ShowUiMode == E_ShowUIMode.HideAll)
                    {
                        HideAllUI(true, View);
                    }
                    else//隐藏所有窗体，不包括当前窗体
                    {
                        if (View.uiType.ShowUiMode == E_ShowUIMode.HideOther)
                        {
                            HideAllUI(false, View);
                        }
                    }
                }
            }
            dicShowUi.Add(View.Uiid,View);
        }

        //销毁窗体
        private void DestroyUI(E_UIID uiId)
        {
            if (dicAllUi.ContainsKey(uiId))
            {
                //存在该窗体，去销毁
                Destroy(dicAllUi[uiId].gameObject);
                dicAllUi.Remove(uiId);
            }
        }

        public void HideAllUI(bool isHideKeepAboveUi, View View)
        {
            if (isHideKeepAboveUi)//隐藏所有窗体
            {
                foreach (View item in dicShowUi.Values)
                {
                    item.HideUI();
                }
                dicShowUi.Clear();
            }
            else//隐藏除前方窗体外的窗体
            {
                List<E_UIID> listRemove=new List<E_UIID>();

                foreach (View item in dicShowUi.Values)
                {
                    if (item.uiType.UiRootType == E_UiRootType.KeepAbove)
                    {
                        continue;
                    }
                    else
                    {
                        item.HideUI();
                        listRemove.Add(item.Uiid);
                    }
                }

                for (int i = 0; i < listRemove.Count; i++)
                {
                    dicShowUi.Remove(listRemove[i]);
                    if (i == listRemove.Count - 1)
                    {
                        //记录上一个窗体的功能
                        beforeUIID = listRemove[i];
                        
                    }
                }
            }
        }
        
        private View GetGaseUI(E_UIID uiid)//判断将要显示的窗体是否已经加载过
        {
            if (dicAllUi.ContainsKey(uiid))
            {
                return dicAllUi[uiid];//之前有加载过，返回uiid的字典
            }
            else
            {
                return null;//之前没有加载过,需要加载
            }
        }

        private Transform GetTheRoot(View View)//获取ui的父节点
        {
            if (View.uiType.UiRootType==E_UiRootType.KeepAbove)
            {
                return keepAboveUIRot;
            }
            else
            {
                return normalUIRoot;
            }
        }

        void Update()
        {
            AutoDestroyUI();
        }

        //自动销毁窗体
        private void AutoDestroyUI()
        {
            if (dicAllUi.Count == 0)
            {
                return;
            }
            List<E_UIID> list = new List<E_UIID>();//储存将要进行删除的UI
            //foreach只读,在遍历的时候不能进行数据增加或者删除
            foreach (KeyValuePair<E_UIID,View> UIItem in dicAllUi)
            {
                if (UIItem.Value.uiType.destroyType == E_DestroyType.NoDestroy)
                {
                    continue;
                }
                else if (UIItem.Value.uiType.destroyType==E_DestroyType.Delay)
                {
                    if (UIItem.Value.destroyTimer.Elapsed.Seconds == UIItem.Value.destroyTime)
                    {
                        //DestroyUI(UIItem.Value.Uiid);
                    }
                }
                else if (UIItem.Value.uiType.destroyType == E_DestroyType.ImmidiatelyDestroy&&!dicShowUi.ContainsKey(UIItem.Value.Uiid))
                {
                    list.Add(UIItem.Value.Uiid);
                }
            }

            for (int i = 0; i < list.Count; i++)
            {
                DestroyUI(list[i]);
            }
        }

    }

}
