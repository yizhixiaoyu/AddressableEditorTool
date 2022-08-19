using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System.IO;
public class CreateFile : Editor
{
    /// <summary>
    /// 创建常用文件夹
    /// </summary>
    [MenuItem("Tools/CreateFolder")]
    public static void CreateFolder()
    {
        //I/0方法 全路径直接创建文件夹
        string path = Application.dataPath + "/";
        Directory.CreateDirectory(path + "Resources");
        Directory.CreateDirectory(path + "Plugins");
        Directory.CreateDirectory(path + "StreamingAssets");
        Directory.CreateDirectory(path + "Editor");
        Directory.CreateDirectory(path + "Scenes");
        Directory.CreateDirectory(path + "Scripts");
        Directory.CreateDirectory(path + "Scripts/CommonTool");
        Directory.CreateDirectory(path + "Scripts/Date");
        Directory.CreateDirectory(path + "Scripts/Globalinstance");
        Directory.CreateDirectory(path + "Scripts/Myui");
        Directory.CreateDirectory(path + "Models");
        Directory.CreateDirectory(path + "UiImage");
        Directory.CreateDirectory(path + "Materials");
        AssetDatabase.Refresh();

        //untiy 的方法
        //if (!AssetDatabase.IsValidFolder("Assets/Resources"))
        //{
        //    AssetDatabase.CreateFolder("Assets", "Resources");
        //}
        //AssetDatabase.CreateFolder("Assets", "Plugins");
        //AssetDatabase.CreateFolder("Assets", "StreamingAssets");
        //AssetDatabase.CreateFolder("Assets", "Editor");
        //AssetDatabase.CreateFolder("Assets", "Scripts");

    }
}
