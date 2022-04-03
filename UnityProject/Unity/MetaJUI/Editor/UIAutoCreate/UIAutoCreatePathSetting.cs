using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XFramework.UI.Editor
{
    public static class UIAutoCreatePathSetting
    {
        /// <summary>
        /// 插件路径
        /// </summary>
        public const string PluginPath = "Assets/3rd/MetaFrameworkUI/";

        /// <summary>
        /// 说明文档路径
        /// </summary>
        public const string ReadMeFilePath = PluginPath + "Editor/UIAutoCreate/readme.txt";

        /// <summary>
        /// 配置路径
        /// </summary>
        public const string UIConfigPath = "Assets/HotUpdateResources/ScriptableObject/UIConfig.asset";

        /// <summary>
        /// 预制体模板路径
        /// </summary>
        public const string PrefabTemplatePath = "Assets/HotUpdateResources/UI/UITemplate";

        /// <summary>
        /// View代码生成配置文件路径
        /// </summary>
        public const string UIViewAutoCreateConfigPath = PluginPath + "Editor/UIAutoCreate/Template/UIViewAutoCreateConfig.asset";

        /// <summary>
        /// 代码模板路径
        /// </summary>
        public const string TemplateFilePath = PluginPath + "Editor/UIAutoCreate/Template/";
        public const string ModelTemplateName = "UIModelTemplate.txt";
        public const string ViewTemplateName = "UIViewTemplate.txt";
        public const string ControlTemplateName = "UIControlTemplate.txt";

        /// <summary>
        /// 代码生成路径
        /// </summary>
        public const string GenerateCsFilePath = "/HotUpdateScripts/Game/UI/";

        /// <summary>
        /// UI代码完整生成路径
        /// </summary>
        /// <returns></returns>
        public static string GetUIGenerateCSFilePath()
        {
            string hotfixPath = Application.dataPath.Replace("/Assets", GenerateCsFilePath);
            return hotfixPath;
        }
    }
}