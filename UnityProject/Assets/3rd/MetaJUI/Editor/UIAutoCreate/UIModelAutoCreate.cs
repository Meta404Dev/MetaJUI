using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace XFramework.UI.Editor
{
    public class UIModelAutoCreate
    {
        public void Create(string uiName, string templatePath, string targetPath)
        {
            TextAsset tempTxt = AssetDatabase.LoadAssetAtPath<TextAsset>(templatePath);
            string tempStr = tempTxt.text;

            string modelClassName = "UIModel" + uiName;

            tempStr = tempStr.Replace("{0}", modelClassName);

            string filePath = targetPath + modelClassName + ".cs";

            if (File.Exists(filePath))
            {
                if (EditorUtility.DisplayDialog("警告", "检测到脚本，是否覆盖", "确定", "取消"))
                {
                    SaveFile(tempStr, filePath);
                }
            }
            else
            {
                SaveFile(tempStr, filePath);
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
    }
}