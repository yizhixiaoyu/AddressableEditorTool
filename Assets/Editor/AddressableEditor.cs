using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Script.ResMgr.Editor
{
    public class AddressableEditor
    {
        public static string path = "/AB_res/";
        public static string Video = "Video";
        public static string Sprite = "Sprite";
        public static string Audio = "Audio";
        public static string Dll = "Dll";
        public static string Scene = "Scene";
        public static string Prefab = "Prefab";

        [MenuItem("Tools/CreateSettings")]
        public static void CreateSettings()
        {
            List<string> groupList = new List<string>();
            groupList.Add(Video);
            groupList.Add(Sprite);
            groupList.Add(Audio);
            groupList.Add(Dll);
            groupList.Add(Scene);
            groupList.Add(Prefab);

            //AddressableAssetSettings yang=AddressableAssetSettingsDefaultObject.CreateInstance<typeof(AddressableAssetSettingsDefaultObject)>();
            //AddressableAssetSettings.Create(path, "First", true, true);
            //temp.CreateAndAddGroupTemplate("First", "First", new Type[] { });
            //AddressableAssetSettingsDefaultObject temp1 = AddressableAssetSettingsDefaultObject.CreateInstance();
            //AddressableAssetSettingsDefaultObject cfg = ScriptableObject.CreateInstance<AddressableAssetSettingsDefaultObject>();
            //以它为基础创建一个Asset
            //AssetDatabase.CreateAsset(cfg, path);
            //保存Asset
            //AssetDatabase.SaveAssets();
            //bool istrue = AddressableAssetSettingsDefaultObject.SettingsExists;
            ///如果不存在默认AddressableAssetSettings面板则创建一个
            AddressableAssetSettings ABsetting;
            if (!AddressableAssetSettingsDefaultObject.SettingsExists)
            {
                ABsetting = AddressableAssetSettingsDefaultObject.GetSettings(true);
                //AddressableAssetSettings temp = new AddressableAssetSettings();
            }
            else
            {
                ABsetting = AddressableAssetSettingsDefaultObject.Settings;
            }
            //yang.groups.Clear();
            AddressableAssetGroup group = ABsetting.DefaultGroup;
            //group.AddSchema(BundledAssetGroupSchema);
            //AddressableAssetGroup Firstgroup = ABsetting.CreateGroup("prefabGroup", false, false, true, new List<AddressableAssetGroupSchema>(), new Type[] { });
            String exefileName;
            AddressableAssetGroup groupitem;
            foreach (string item in groupList)
            {
                //判断是否已经创建Group
                if (!ABsetting.FindGroup(item))
                {
                    groupitem = ABsetting.CreateGroup(item, false, false, true, new List<AddressableAssetGroupSchema>(), new Type[] { });
                }
                else
                {
                    groupitem = ABsetting.FindGroup(item);
                }
                DirectoryInfo direction = new DirectoryInfo(Application.dataPath + path + item);
                //获取文件夹，exportPath是文件夹的路径
                FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
                //*是获取这个文件夹下的所有文件，
                for (int i = 0; i < files.Length; i++)
                {
                    //判断文件的后缀
                    //if (files[i].Name.EndsWith(".FBX"))
                    exefileName = files[i].FullName;
                    //系统路径修改为unity路径
                    exefileName = exefileName.Remove(0, exefileName.IndexOf("Assets"));
                    //获取到guid
                    string tempguid = AssetDatabase.AssetPathToGUID(exefileName);
                    //创建并移入group
                    ABsetting.CreateOrMoveEntry(tempguid, groupitem);
                }
                ABsetting.AddLabel(item);
                foreach (AddressableAssetEntry entryitem in groupitem.entries)
                {
                    entryitem.SetLabel(item, true);
                    entryitem.SetAddress(entryitem.AssetPath.Substring(entryitem.AssetPath.LastIndexOf("/") + 1, Mathf.Abs(entryitem.AssetPath.LastIndexOf("/") - entryitem.AssetPath.LastIndexOf(".")) - 1));
                    //entryitem.SetAddress(entryitem);
                    Debug.LogError("名字是什么" + entryitem.AssetPath.Substring(entryitem.AssetPath.LastIndexOf("/") + 1, Mathf.Abs(entryitem.AssetPath.LastIndexOf("/") - entryitem.AssetPath.LastIndexOf(".")) - 1));
                    Debug.LogError(entryitem.address + "  :group:   " + entryitem.AssetPath + " 名字 :" + item.ToString());
                    //Assets/AB_res/Prefab/Sphere.prefab  :group:   Assets/AB_res/Prefab/Sphere.prefab
                }
            }

            //group.entries.Add();
            //AddressableAssetEntry
            //string exportPath = "Assets/AB_res/Prefabs/Cube.prefab";
            //string guid = AssetDatabase.AssetPathToGUID(exportPath);
            //Debug.Log(guid + "       :guid");
            //ABsetting.CreateOrMoveEntry(guid, group);

            //string exefileName = "";
            //GameObject[] game = Resources.LoadAll<GameObject>(exportPath);
            //Debug.LogError("  :group:   ");
            //GameObject[] game = Resources.LoadAll<GameObject>(exportPath);
            //GameObject game1 = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Character.prefab", typeof(GameObject)) as GameObject;
            //string path = Application.dataPath + "/AddressSetting";
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [MenuItem("AddressableEditor/SetGroup => StaticContent")]
        public static void SetStaticContentGroup()
        {
            //ABsetting = AddressableAssetSettingsDefaultObject.Settings;
            AddressableAssetSettings ABsetting = AddressableAssetSettingsDefaultObject.Settings;
            foreach (AddressableAssetGroup groupAsset in ABsetting.groups)
            {
                groupAsset.AddSchema(new ContentUpdateGroupSchema());
                groupAsset.AddSchema(new BundledAssetGroupSchema());
                for (int i = 0; i < groupAsset.Schemas.Count; i++)
                {
                    var schema = groupAsset.Schemas[i];
                    if (schema is ContentUpdateGroupSchema)
                    {
                        (schema as ContentUpdateGroupSchema).StaticContent = true;
                    }
                    else if (schema is BundledAssetGroupSchema)
                    {
                        var bundledAssetGroupSchema = (schema as BundledAssetGroupSchema);
                        bundledAssetGroupSchema.BuildPath.SetVariableByName(groupAsset.Settings, AddressableAssetSettings.kLocalBuildPath);
                        bundledAssetGroupSchema.LoadPath.SetVariableByName(groupAsset.Settings, AddressableAssetSettings.kLocalLoadPath);
                    }
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [MenuItem("AddressableEditor/Build All Content")]
        public static void BuildContent()
        {
            AddressableAssetSettings.BuildPlayerContent();
        }

        [MenuItem("AddressableEditor/Prepare Update Content")]
        public static void CheckForUpdateContent()
        {
            //与上次打包做资源对比
            //string buildPath = ContentUpdateScript.GetContentStateDataPath(false);
            //var m_Settings = AddressableAssetSettingsDefaultObject.Settings;
            //List<AddressableAssetEntry> entrys = ContentUpdateScript.GatherModifiedEntries(m_Settings, buildPath);
            //if (entrys.Count == 0) return;
            //StringBuilder sbuider = new StringBuilder();
            //sbuider.AppendLine("Need Update Assets:");
            //foreach (var _ in entrys)
            //{
            //    sbuider.AppendLine(_.address);
            //}
            //Debug.Log(sbuider.ToString());

            ////将被修改过的资源单独分组
            //var groupName = string.Format("UpdateGroup_{0}", DateTime.Now.ToString("yyyyMMdd"));
            //ContentUpdateScript.CreateContentUpdateGroup(m_Settings, entrys, groupName);
        }

        [MenuItem("AddressableEditor/BuildUpdate")]
        public static void BuildUpdate()
        {
            var path = ContentUpdateScript.GetContentStateDataPath(false);
            var m_Settings = AddressableAssetSettingsDefaultObject.Settings;
            AddressablesPlayerBuildResult result = ContentUpdateScript.BuildContentUpdate(AddressableAssetSettingsDefaultObject.Settings, path);
            Debug.Log("BuildFinish path = " + m_Settings.RemoteCatalogBuildPath.GetValue(m_Settings));
        }

        [MenuItem("AddressableEditor/Test")]
        public static void Test()
        {
            Debug.Log("BuildPath = " + Addressables.BuildPath);
            Debug.Log("PlayerBuildDataPath = " + Addressables.PlayerBuildDataPath);
            Debug.Log("RemoteCatalogBuildPath = " + AddressableAssetSettingsDefaultObject.Settings.RemoteCatalogBuildPath.GetValue(AddressableAssetSettingsDefaultObject.Settings));
        }
    }
}
