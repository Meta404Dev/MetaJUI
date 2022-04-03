using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MetaFramework.Singleton;
using JEngine.Core;

namespace MetaFramework.UI
{
    /// <summary>
    /// UI的命名规则：
    /// Prefab：     UIXXX
    /// Control：    UIXXX
    /// Model：      UIModelXXX
    /// View：       UIViewXXX
    /// </summary>
    public class UIManager : SingletonTemplate<UIManager>
    {
        public const string UI_RES_PATH = "Assets/HotUpdateResources/UI/{0}.prefab";

        /// <summary>
        /// UI根节点
        /// </summary>
        private GameObject uiRootGo;

        /// <summary>
        /// UI缓存
        /// </summary>
        private Dictionary<string, IUIBase> uiCacheDic;

        /// <summary>
        /// UI栈
        /// </summary>
        private Stack<IUIBase> uiStack;

        /// <summary>
        /// 普通UI列表
        /// </summary>
        private List<IUIBase> uiList;

        public UIManager()
        {
            uiCacheDic = new Dictionary<string, IUIBase>();
            uiStack = new Stack<IUIBase>();
            uiList = new List<IUIBase>();

            uiRootGo = CreateUIRoot();
        }


        public IUIBase Get(string uiName)
        {
            IUIBase ui = null;
            if (!uiCacheDic.TryGetValue(uiName, out ui))
            {
                Debug.LogError("can not find the ui from cache:" + uiName);
                return null;
            }

            return ui;
        }

        /// <summary>
        /// 普通打开一个UI
        /// </summary>
        /// <param name="uiName"></param>
        /// <param name="onComplete"></param>
        /// <returns></returns>
        public void OpenNormal(string uiName, Action<IUIBase> onComplete, params object[] args)
        {
            //打开UI，优先从缓存取
            IUIBase ui = null;
            if (!uiCacheDic.TryGetValue(uiName, out ui))
            {
                LoadUI(uiName, (ui) =>
                {
                    OnOpenUI(ui, onComplete, args);
                });
            }
            else
            {
                OnOpenUI(ui, onComplete, args);
            }

            void OnOpenUI(IUIBase ui, Action<IUIBase> onComplete, params object[] args)
            {
                uiList.Add(ui);
                ui.OnEnter(args);
                ui.uiGo.SetActive(true);

                onComplete?.Invoke(ui);
            }
        }

        /// <summary>
        /// 普通关闭一个UI
        /// </summary>
        /// <param name="uiName"></param>
        public void CloseNormal(string uiName)
        {
            if (uiList.Count <= 0) return;

            IUIBase ui = uiList.Find(
                delegate (IUIBase ui)
                {
                    return ui.uiName.Equals(uiName);
                });

            if (ui == null)
            {
                Debug.LogError("can not find the ui:" + uiName);
                return;
            }

            uiList.Remove(ui);
            ui.uiGo.SetActive(false);
            ui.OnExit();
        }

        /// <summary>
        /// 通过栈打开一个UI
        /// </summary>
        /// <param name="uiName"></param>
        /// <param name="onComplete"></param>
        public void OpenStack(string uiName, Action<IUIBase> onComplete, params object[] args)
        {
            //栈顶界面暂停
            if (uiStack.Count > 0)
            {
                IUIBase topUI = uiStack.Peek();
                topUI.OnPause();
            }

            //打开UI，优先从缓存取
            IUIBase ui = null;
            if (!uiCacheDic.TryGetValue(uiName, out ui))
            {
                LoadUI(uiName, (ui) =>
                {
                    OnOpenUI(ui, onComplete, args);
                });
            }
            else
            {
                OnOpenUI(ui, onComplete, args);
            }

            void OnOpenUI(IUIBase ui, Action<IUIBase> onComplete, params object[] args)
            {
                //入栈
                uiStack.Push(ui);
                ui.uiGo.SetActive(true);
                ui.OnEnter(args);

                onComplete.Invoke(ui);
            }
        }

        /// <summary>
        /// 关闭栈顶的UI
        /// </summary>
        public void CloseStack()
        {
            if (uiStack.Count <= 0) return;

            IUIBase ui = uiStack.Pop();
            ui.uiGo.SetActive(false);
            ui.OnExit();

            //栈顶界面恢复
            if (uiStack.Count > 0)
            {
                IUIBase topUI = uiStack.Peek();
                topUI.OnResume();
            }
        }

        /// <summary>
        /// 关闭所有界面
        /// </summary>
        public void CloseAll()
        {
            while (uiStack.Count > 0)
            {
                CloseStack();
            }

            for (int i = uiList.Count - 1; i >= 0; i--)
            {
                CloseNormal(uiList[i].uiName);
            }
        }

        /// <summary>
        /// 清除所有界面，包括缓存
        /// </summary>
        public void Clear()
        {
            CloseAll();

            foreach (var uiCache in uiCacheDic.Values)
            {
                GameObject.Destroy(uiCache.uiGo);
            }

            uiCacheDic.Clear();
        }

        private GameObject CreateUIRoot()
        {
            GameObject go = new GameObject("UIRoot");
            GameObject.DontDestroyOnLoad(go);

            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.layer = LayerMask.NameToLayer("UI");

            var canvas = go.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 2;

            var canvasScaler = go.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920, 1080);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

            go.AddComponent<GraphicRaycaster>();

            //create ui node
            foreach (UILayer type in Enum.GetValues(typeof(UILayer)))
            {
                GameObject node = new GameObject(type.ToString());
                node.transform.parent = go.transform;
                node.transform.localPosition = Vector3.zero;
                node.transform.localScale = Vector3.one;
            }

            //add event system
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.transform.parent = go.transform;
            eventSystem.transform.localPosition = Vector3.zero;
            eventSystem.transform.localScale = Vector3.one;

            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
            eventSystem.AddComponent<BaseInput>();

            return go;
        }

        private void LoadUI(string uiName, Action<IUIBase> onComplete)
        {
            // create ui gameobject
            string uiPath = string.Format(UI_RES_PATH, uiName);

            new JPrefab(uiPath, (result, prefab) =>
            {
                if (!result)
                {
                    throw new Exception("load ui prefab failed:" + uiName);
                }


                // get ui class name
                string uiOriginName = uiName.Replace("UI", "");
                string uiModelName = "UIModel" + uiOriginName;
                string uiViewName = "UIView" + uiOriginName;


                // create ui class
                Type uiType = Type.GetType(uiName);
                Type uiModelType = Type.GetType(uiModelName);
                Type uiViewType = Type.GetType(uiViewName);

                IUIBase ui = Activator.CreateInstance(uiType) as IUIBase;
                IUIModel uiModel = Activator.CreateInstance(uiModelType) as IUIModel;
                IUIView uiView = Activator.CreateInstance(uiViewType) as IUIView;


                var parent = uiRootGo.transform.Find(ui.GetLayer().ToString());
                if (parent == null) throw new Exception("can not find the ui layer:" + ui.GetLayer());

                GameObject uiGo = GameObject.Instantiate(prefab.Instance, parent);
                uiGo.name = uiName;
                uiGo.transform.localPosition = Vector3.zero;
                uiGo.transform.localScale = Vector3.one;


                // init class
                uiView.Init(uiGo);
                ui.uiName = uiName;
                ui.uiGo = uiGo;
                ui.uiModel = uiModel;
                ui.uiView = uiView;


                // add to cache
                uiCacheDic.Add(uiName, ui);

                // complete
                onComplete?.Invoke(ui);
            });
        }







    }
}