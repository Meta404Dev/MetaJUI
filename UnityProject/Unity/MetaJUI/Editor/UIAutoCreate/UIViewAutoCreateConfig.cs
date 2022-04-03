using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XFramework.UI.Editor
{
    [Serializable]
    public class UIViewAutoCreateInfo
    {
        /// <summary>
        /// 属性名
        /// </summary>
        public string propName;

        /// <summary>
        /// 组件名
        /// </summary>
        public string comName;
    }
    [Serializable, CreateAssetMenu(menuName = "UI/CreateUIViewAutoCreateConfig")]
    public class UIViewAutoCreateConfig : ScriptableObject
    {
        public List<UIViewAutoCreateInfo> uiInfoList;
    }
}
