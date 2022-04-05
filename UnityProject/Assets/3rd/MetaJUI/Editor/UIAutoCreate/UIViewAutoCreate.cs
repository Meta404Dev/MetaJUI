using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace XFramework.UI.Editor
{
    public class UIViewAutoCreate
    {
        public class ViewPropAndCom
        {
            public string strProp;
            public string strGetCom;
        }

        private UIViewAutoCreateConfig config;

        private GameObject uiRootGo;

        private Dictionary<string, ViewPropAndCom> allPropsDic = new Dictionary<string, ViewPropAndCom>();

        public void Create(string uiName, GameObject uiRootGo, string templatePath, string targetPath)
        {
            this.uiRootGo = uiRootGo;
            allPropsDic.Clear();

            config = AssetDatabase.LoadAssetAtPath<UIViewAutoCreateConfig>(UIAutoCreatePathSetting.UIViewAutoCreateConfigPath);

            string tempStrFile = AssetDatabase.LoadAssetAtPath<TextAsset>(templatePath).text;

            FindGoChild(uiRootGo.transform, true);
            if (allPropsDic.Count <= 0)
            {
                Debug.Log("<color=#ff0000>组件数量为0，请确认组件命名是否正确！</color>");
            }

            StringBuilder strTotalProps = new StringBuilder();
            StringBuilder strTotalGetComs = new StringBuilder();
            foreach (var prop in allPropsDic)
            {
                strTotalProps.Append(prop.Value.strProp);
                strTotalGetComs.Append(prop.Value.strGetCom);
            }

            string viewClassName = "UIView" + uiName;

            tempStrFile = tempStrFile.Replace("{0}", viewClassName);
            tempStrFile = tempStrFile.Replace("{1}", strTotalProps.ToString());
            tempStrFile = tempStrFile.Replace("{2}", strTotalGetComs.ToString());

            string filePath = targetPath + viewClassName + ".cs";

            if (File.Exists(filePath))
            {
                if (EditorUtility.DisplayDialog("警告", "检测到脚本，是否覆盖", "确定", "取消"))
                {
                    SaveFile(tempStrFile, filePath);
                }
            }
            else
            {
                SaveFile(tempStrFile, filePath);
            }
        }

        public void SaveFile(string str, string filePath)
        {
            if (File.Exists(filePath)) File.Delete(filePath);

            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.Write(str);
                }
            }
            Debug.Log("创建成功: " + filePath);
            AssetDatabase.Refresh();
        }


        private void FindGoChild(Transform ts, bool isRoot)
        {
            if (!isRoot)
            {
                CheckUIView(ts);
                if (ts.name.StartsWith("UICommon")) return;
            }

            for (int i = 0; i < ts.childCount; i++)
            {
                FindGoChild(ts.GetChild(i), false);
            }
        }



        private UIViewAutoCreateInfo FindComponentByConfig(string transName)
        {
            if (transName.StartsWith("UICommon"))
            {
                var info = new UIViewAutoCreateInfo()
                {
                    comName = transName,
                };
                return info;
            }

            for (int i = 0; i < config.uiInfoList.Count; i++)
            {
                var info = config.uiInfoList[i];

                if (transName.Contains(info.propName)) return info;
            }

            return null;
        }

        private void CheckUIView(Transform child)
        {
            var info = FindComponentByConfig(child.name);

            if (info == null) return;

            //get final name
            string finalPropName;
            if (child.name.StartsWith("UICommon"))
            {
                finalPropName = child.name;
            }
            else
            {
                string[] childNames = child.name.Split('_');
                finalPropName = childNames[0].ToLower() + childNames[1];
            }

            string strTempProp = string.Format("\tpublic {0} {1};\n", info.comName, finalPropName);

            string path = GetPath(child);
            string strTempCom = string.Format("\t\t{0} = go.transform.Find(\"{1}\").GetComponent<{2}>();\n",
                finalPropName, path, info.comName);

            ViewPropAndCom view;
            if (allPropsDic.TryGetValue(finalPropName, out view))
            {
                throw new System.Exception("组件重名！ " + path);
            }

            var viewPropAndCom = new ViewPropAndCom()
            {
                strProp = strTempProp,
                strGetCom = strTempCom,
            };
            allPropsDic.Add(finalPropName, viewPropAndCom);
        }
        private string GetPath(Transform transform)
        {
            string path = transform.name;
            while (transform.parent != uiRootGo.transform)
            {
                transform = transform.parent;
                path = transform.name + "/" + path;
            }
            return path;
        }
    }
}