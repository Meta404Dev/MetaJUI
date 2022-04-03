using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace XFramework.UI.Editor
{
    public class UIScriptAutoCreateEditorWindow : EditorWindow
    {
        private string NewUIName;

        private GameObject uiRootGo;

        private static string readMeText;



        [MenuItem("MetaTools/UI自动生成器 #&%U", false, 999)]
        static void ShowEditor()
        {
            UIScriptAutoCreateEditorWindow window = GetWindow<UIScriptAutoCreateEditorWindow>();
            window.minSize = new Vector2(600, 300);
            window.titleContent.text = "UI自动生成器";

            var readMe = AssetDatabase.LoadAssetAtPath<TextAsset>(UIAutoCreatePathSetting.ReadMeFilePath);
            readMeText = readMe.text;
        }

        private void OnGUI()
        {
            #region GUIStyle 设置
            Color fontColor = new Color(179f / 255f, 179f / 255f, 179f / 255f, 1f);

            //GUIStyle gl = "Toggle";
            //gl.margin = new RectOffset(0, 100, 0, 0);

            GUIStyle titleStyle = new GUIStyle() { fontSize = 18, alignment = TextAnchor.MiddleCenter };
            titleStyle.normal.textColor = fontColor;

            GUIStyle sonTittleStyle = new GUIStyle() { fontSize = 15, alignment = TextAnchor.MiddleCenter };
            sonTittleStyle.normal.textColor = fontColor;

            GUIStyle leftStyle = new GUIStyle() { fontSize = 15, alignment = TextAnchor.MiddleLeft };
            leftStyle.normal.textColor = fontColor;

            GUIStyle littoleStyle = new GUIStyle() { fontSize = 13, alignment = TextAnchor.MiddleCenter };
            littoleStyle.normal.textColor = fontColor;
            #endregion

            GUILayout.BeginArea(new Rect(0, 0, 600, 1200));
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            EditorGUILayout.TextArea(readMeText, leftStyle, GUILayout.Width(600));
            GUILayout.EndHorizontal();


            GUILayout.Space(50);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("预制体自动生成设置", titleStyle, GUILayout.Width(600));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("新UI的名字", leftStyle, GUILayout.Width(150));
            NewUIName = EditorGUILayout.TextField(NewUIName, GUILayout.Width(350));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("生成UI预制体", GUILayout.Width(600), GUILayout.Height(30)))
            {
                CreateUIPrefab();
            }
            GUILayout.EndHorizontal();



            GUILayout.Space(50);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("代码自动生成设置", titleStyle, GUILayout.Width(600));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("View根节点:", leftStyle, GUILayout.Width(150));
            uiRootGo = (GameObject)EditorGUILayout.ObjectField(uiRootGo, typeof(GameObject), true);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("生成View代码", GUILayout.Width(600), GUILayout.Height(30)))
            {
                CreateUIView();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("生成MVC代码", GUILayout.Width(600), GUILayout.Height(30)))
            {
                CreateMVC();
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        private void CreateUIPrefab()
        {
            if (string.IsNullOrEmpty(NewUIName)) throw new System.Exception("请输入UI名字");

            string newPrefabPath = UIAutoCreatePathSetting.PrefabTemplatePath.Replace("UITemplate", NewUIName);

            //copy prefab
            string strPrefab = ".prefab";
            string originPrefabFullPath = Application.dataPath + UIAutoCreatePathSetting.PrefabTemplatePath + strPrefab;
            string newPrefabFullPath = Application.dataPath + newPrefabPath + strPrefab;
            originPrefabFullPath = originPrefabFullPath.Replace("/AssetsAssets", "/Assets");
            newPrefabFullPath = newPrefabFullPath.Replace("/AssetsAssets", "/Assets");

            //copy meta
            string strMeta = ".meta";
            string originMetaFullPath = originPrefabFullPath + strMeta;
            string newMetaFullPath = newPrefabFullPath + strMeta;

            bool result = false;
            if (File.Exists(newPrefabFullPath))
            {
                if (EditorUtility.DisplayDialog("警告", "检测到UI预制体，是否覆盖", "确定", "取消"))
                {
                    File.Copy(originPrefabFullPath, newPrefabFullPath, true);
                    File.Copy(originMetaFullPath, newMetaFullPath, true);
                    result = true;
                }
            }
            else
            {
                File.Copy(originPrefabFullPath, newPrefabFullPath);
                File.Copy(originMetaFullPath, newMetaFullPath);
                result = true;
            }

            if (result)
            {
                Debug.Log("UI创建成功: " + newPrefabPath);
                AssetDatabase.Refresh();

                string newUIFullPath = newPrefabPath + strPrefab;
                uiRootGo = AssetDatabase.LoadAssetAtPath<GameObject>(newUIFullPath);
            }
        }

        private void CreateMVC()
        {
            CreateUIView();
            CreateUIModel();
            CreateUIControl();
        }
        private void CreateUIView()
        {
            if (uiRootGo == null) throw new System.Exception("请拖入需要生成的UI预制体");

            string uiName = GetUIName();
            string tempPath = UIAutoCreatePathSetting.TemplateFilePath + UIAutoCreatePathSetting.ViewTemplateName;
            string targetPath = GetTargetGeneratePath(uiName);
            CheckTargetPath(targetPath);
            new UIViewAutoCreate().Create(uiName, uiRootGo, tempPath, targetPath);
        }
        private void CreateUIControl()
        {
            if (uiRootGo == null) throw new System.Exception("请拖入需要生成的UI预制体");

            string uiName = GetUIName();
            string tempPath = UIAutoCreatePathSetting.TemplateFilePath + UIAutoCreatePathSetting.ControlTemplateName;
            string targetPath = GetTargetGeneratePath(uiName);
            CheckTargetPath(targetPath);
            new UIControlAutoCreate().Create(uiName, tempPath, targetPath);
        }
        private void CreateUIModel()
        {
            if (uiRootGo == null) throw new System.Exception("请拖入需要生成的UI预制体");

            string uiName = GetUIName();
            string tempPath = UIAutoCreatePathSetting.TemplateFilePath + UIAutoCreatePathSetting.ModelTemplateName;
            string targetPath = GetTargetGeneratePath(uiName);
            CheckTargetPath(targetPath);
            new UIModelAutoCreate().Create(uiName, tempPath, targetPath);
        }

        private string GetTargetGeneratePath(string uiName)
        {
            return UIAutoCreatePathSetting.GetUIGenerateCSFilePath() + "UI" + uiName + "/";
        }

        private void CheckTargetPath(string targetPath)
        {
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }
        }

        private string GetUIName()
        {
            string uiName = uiRootGo.name.Replace("UI", "");
            return uiName;
        }
    }
}