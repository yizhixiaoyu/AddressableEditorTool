using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
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
        public static string Assets = "Assets/AB_res";
        public static string path = "/AB_res/";
        public static string Video = "Video";
        public static string Sprite = "Sprite";
        public static string Audio = "Audio";
        public static string Dll = "Dll";
        public static string Scene = "Scene";
        public static string Prefab = "Prefab";
        public static string Material = "Material";

        [MenuItem("Tools/CreateDefault")]
        public static void createfiles()
        {
            //AssetDatabase.CreateFolder(Application.dataPath + path, Video);
            //AssetDatabase.CreateFolder(Application.dataPath + path, Sprite);
            //AssetDatabase.CreateFolder(Application.dataPath + path, Audio);
            //AssetDatabase.CreateFolder(Application.dataPath + path, Dll);
            //AssetDatabase.CreateFolder(Application.dataPath + path, Scene);
            //Debug.Log(Path.GetFileNameWithoutExtension(Application.dataPath + path + Material));
            //Debug.Log(Path.GetFileName(Application.dataPath + path + Material));

            //Debug.Log(Assets + path + Material);
            if (!AssetDatabase.IsValidFolder(Assets + path + Material))
            {
                AssetDatabase.CreateFolder(Assets, Material);
            }
            //Debug.Log(Path.Combine(Assets, Material));

            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/md5加密")]
        public static void md5()
        {
            CustomWindow custom = new CustomWindow();
            custom.Show();
        }

        [MenuItem("Tools/CreateSettings")]
        public static void CreateSettings()
        {
            List<string> groupList = new List<string>();
            //groupList.Add(Video);
            //groupList.Add(Sprite);
            //groupList.Add(Audio);
            //groupList.Add(Dll);
            //groupList.Add(Scene);
            //groupList.Add(Prefab);
            //FileInfo[] files = getfiles(Application.dataPath + path);
            DirectoryInfo direction = new DirectoryInfo(Assets);
            //GetDirectories
            DirectoryInfo[] Directoryfiles = direction.GetDirectories("*", SearchOption.AllDirectories);
            //Debug.LogError(files.Length + ":长度");
            foreach (DirectoryInfo item in Directoryfiles)
            {
                Debug.Log("Group名字是:" + item.Name);
                groupList.Add(item.Name);
            }
            Debug.LogError("gooup的长度:" + groupList.Count);
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
            //AddressableAssetGroup group = ABsetting.DefaultGroup;
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

                //DirectoryInfo direction = new DirectoryInfo(Application.dataPath + path + item);
                //获取文件夹，exportPath是文件夹的路径
                //FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
                //获取文件夹下的文件
                FileInfo[] filesitem = getfiles(Application.dataPath + path + item);
                //*是获取这个文件夹下的所有文件，
                for (int i = 0; i < filesitem.Length; i++)
                {
                    //判断文件的后缀
                    //if (files[i].Name.EndsWith(".FBX"))
                    exefileName = filesitem[i].FullName;
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

                    entryitem.SetAddress(Path.GetFileNameWithoutExtension(entryitem.AssetPath));
                    //entryitem.SetAddress(entryitem.AssetPath.Substring(entryitem.AssetPath.LastIndexOf("/") + 1, Mathf.Abs(entryitem.AssetPath.LastIndexOf("/") - entryitem.AssetPath.LastIndexOf(".")) - 1));
                    //entryitem.SetAddress(entryitem);
                    //entryitem.address
                    //Debug.LogError("名字是什么" + entryitem.AssetPath.Substring(entryitem.AssetPath.LastIndexOf("/") + 1, Mathf.Abs(entryitem.AssetPath.LastIndexOf("/") - entryitem.AssetPath.LastIndexOf(".")) - 1));
                    //Debug.LogError(entryitem.address + "  :group:   " + entryitem.AssetPath + " 名字 :" + item.ToString());
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

        public static FileInfo[] getfiles(string path)
        {
            DirectoryInfo direction = new DirectoryInfo(path);
            //获取文件夹，exportPath是文件夹的路径
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

            return files;
        }
        //[MenuItem("Assets/添加到Group", priority = 0)]
        //public static void setcontentGroup()
        //{
        //    EditorWindow window = EditorWindow.GetWindow<CustomWindow>();
        //    window.minSize = new Vector2(400, 450);
        //    window.maxSize = new Vector2(400, 450);
        //    window.Show();
        //}



        [MenuItem("AddressableEditor/SetGroup => StaticContent")]//设置为不可更新状态
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

            //AddAndSetActiveDataBuilder();
        }
        //public static void AddAndSetActiveDataBuilder(IDataBuilder dataBuilder)
        //{
        //    if (AddressableAssetSettingsDefaultObject.Settings.AddDataBuilder(dataBuilder))
        //    {
        //        AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilderIndex =
        //            AddressableAssetSettingsDefaultObject.Settings.DataBuilders.Count - 1;
        //    }
        //}


        [MenuItem("AddressableEditor/Prepare Update Content")]
        public static void CheckForUpdateContent()
        {
            //与上次打包做资源对比
            string buildPath = ContentUpdateScript.GetContentStateDataPath(false);
            var m_Settings = AddressableAssetSettingsDefaultObject.Settings;
            List<AddressableAssetEntry> entrys = ContentUpdateScript.GatherModifiedEntries(m_Settings, buildPath);
            if (entrys.Count == 0) return;
            StringBuilder sbuider = new StringBuilder();
            sbuider.AppendLine("Need Update Assets:");
            foreach (var _ in entrys)
            {
                sbuider.AppendLine(_.address);
            }
            Debug.Log(sbuider.ToString());

            //将被修改过的资源单独分组
            var groupName = string.Format("UpdateGroup_{0}", DateTime.Now.ToString("yyyyMMdd"));
            ContentUpdateScript.CreateContentUpdateGroup(m_Settings, entrys, groupName);
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

    //[MenuItem("md5加密")]
    public class CustomWindow : EditorWindow
    {
        private string configLableName;
        //private int configCodeIndex;
        public void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            configLableName = EditorGUILayout.TextField("Lable", configLableName, GUILayout.MaxWidth(400));
            //configCodeIndex = EditorGUILayout.IntField("Lable", configCodeIndex, GUILayout.MaxWidth(400));
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("生成MD5"))
            {
                if (!String.IsNullOrEmpty(configLableName))
                {
                    Encrypt(configLableName, 16);
                }

                //Debug.Log($"当前生成的配置文件名称为:{configLableName}");
            }
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">加密字符</param>
        /// <param name="code">加密位数16/32</param>
        /// <returns></returns>
        public static string Encrypt(string str, int code)
        {
            string strEncrypt = string.Empty;
            if (code == 16)
            {
                strEncrypt = Hash(str).Substring(8, 16);
            }

            if (code == 32)
            {
                strEncrypt = Hash(str);
            }
            return strEncrypt;
        }
        /// <summary>
        /// 32位MD5加密（小写）
        /// </summary>
        /// <param name="input">输入字段</param>
        /// <returns></returns>
        public static string Hash(string input)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }




    [LabelText("批量加载资源到Ad包资源")]
    public class MySimpleEditorWindow : OdinEditorWindow
    {
        public static string path = "/AB_res/";
        public static string AB_resPath = "Assets/AB_res";


        [LabelText("父类文件夹"), LabelWidth(80), ReadOnly]
        public string folder;

        /// <summary>
        /// 所选文件的一组 guid
        /// </summary>
        private string[] itemsGuid;

        private int[] instanceid;
        //[Property]
        [HorizontalGroup("GroupName", 200), LabelWidth(80), Tooltip("组自己定义或者选择已有"), SuffixLabel("自定义或选择", true)]
        public string GroupName;
        [HorizontalGroup("GroupName", 200), ValueDropdown("listGroupname"), OnValueChanged("setgroupname"), HideLabel]
        public string currentgroup;
        private List<string> listGroupname;


        [HorizontalGroup("label", 200), Tooltip("标签自己定义或者选择已有"), LabelWidth(80), SuffixLabel("自定义或选择", true)]
        public string label;
        [HorizontalGroup("label", 200), ValueDropdown("listlabel"), OnValueChanged("setlael"), HideLabel]
        public string currentlabel;
        private List<string> listlabel;

        [LabelWidth(200), LabelText("是否移动资源到Assets/AB_res/目录下")]
        [InfoBox("不可以有同名同类型的文件,否则会被覆盖", InfoMessageType = InfoMessageType.Error)]
        public bool istoggle = false;
        [ShowIfGroup("istoggle")]
        [HorizontalGroup("istoggle/path", 200), Tooltip("自定义目录或选择已有"), LabelWidth(80), SuffixLabel("自定义或选择", true)]
        public string filepath;//自定义目录 都是在/AB_res/目录下的位置
        [ShowIfGroup("istoggle")]
        [HorizontalGroup("istoggle/path", 200), ValueDropdown("newpathlist"), OnValueChanged("setpath"), HideLabel]
        public string currentpath;
        private List<string> newpathlist = new List<string>();
        private Dictionary<string, string> ABrespathlist = new Dictionary<string, string>();

        [Button("添加资源"), LabelWidth(200)]
        public void addres()
        {

            #region 物体移动到addressable的group的批量操作

            if (String.IsNullOrEmpty(GroupName))
            {
                //弹框 没有
                Debug.LogError("GroupName不能为空");
                return;
            }



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

            AddressableAssetGroup groupitem;
            ///判断是否创建组
            if (!ABsetting.FindGroup(GroupName))
            {
                groupitem = ABsetting.CreateGroup(GroupName, false, false, true, new List<AddressableAssetGroupSchema>(), new Type[] { });
            }
            else
            {
                groupitem = ABsetting.FindGroup(GroupName);
            }


            //获取所选的文件夹下的所有文件

            //DirectoryInfo direction = new DirectoryInfo(Application.dataPath);
            //FileInfo[] files = direction.GetFiles("folder");
            //AssetDatabase.GetAssetPath
            //string[] guids1 = AssetDatabase.FindAssets(folder);

            //选择文件下的所有物体移动到对应的addressable的Group下
            foreach (string guid in itemsGuid)
            {
                //Debug.Log(guid + ":guid1");
                Debug.Log(AssetDatabase.GUIDToAssetPath(guid));

                //AssetDatabase.Contains();
                DirectoryInfo direction = new DirectoryInfo(AssetDatabase.GUIDToAssetPath(guid));

                if (direction.Exists)//是否为(文件夹)目录文件 不是文件夹 就是文件
                {
                    //*是获取这个文件夹下的所有文件，
                    FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
                    Debug.Log("文件的长度:" + files.Length);
                    //多选文件夹
                    for (int i = 0; i < files.Length; i++)
                    {
                        //判断文件的后缀
                        //if (files[i].Name.EndsWith(".FBX"))
                        //AssetDatabase.GetAssetPath(files[i].FullName);
                        string exefileName = files[i].FullName;
                        //系统路径修改为unity路径
                        exefileName = exefileName.Remove(0, exefileName.IndexOf("Assets"));
                        //获取到guid
                        string tempguid = AssetDatabase.AssetPathToGUID(exefileName);

                        ABsetting.RemoveAssetEntry(tempguid);

                        //创建并移入group
                        ABsetting.CreateOrMoveEntry(tempguid, groupitem);
                    }
                }
                else
                {
                    //多选文件
                    ABsetting.RemoveAssetEntry(guid);
                    //创建并移入group
                    ABsetting.CreateOrMoveEntry(guid, groupitem);
                }
            }
            #endregion

            //添加标签
            if (!String.IsNullOrEmpty(label))
            {
                if (!listlabel.Contains(label))
                {
                    ABsetting.AddLabel(label);
                }
            }

            foreach (AddressableAssetEntry entryitem in groupitem.entries)
            {
                entryitem.SetAddress(entryitem.AssetPath.Substring(entryitem.AssetPath.LastIndexOf("/") + 1, Mathf.Abs(entryitem.AssetPath.LastIndexOf("/") - entryitem.AssetPath.LastIndexOf(".")) - 1));

                if (String.IsNullOrEmpty(label)) continue;
                entryitem.SetLabel(label, true);
                //entryitem.SetAddress(entryitem);

                //Debug.LogError("名字是什么" + entryitem.AssetPath.Substring(entryitem.AssetPath.LastIndexOf("/") + 1, Mathf.Abs(entryitem.AssetPath.LastIndexOf("/") - entryitem.AssetPath.LastIndexOf(".")) - 1));
                //Debug.LogError(entryitem.address + "  :group:   " + entryitem.AssetPath + " 名字 :" + label.ToString());
                //Assets/AB_res/Prefab/Sphere.prefab  :group:   Assets/AB_res/Prefab/Sphere.prefab
            }



            //是否移动
            if (!istoggle)
            {
                AssetDatabase.Refresh();
                return;
            }

            ///
            /// 移动文件到目标下
            ///
            //string path = "/AB_res/";
            if (!ABrespathlist.ContainsKey(filepath) && !ABrespathlist.ContainsValue(filepath))
            {
                Debug.Log("是否存在目录");
                //创建一个空文件夹
                filepath = Path.GetFileNameWithoutExtension(filepath);
                //if (!direction.Exists)
                AssetDatabase.CreateFolder(AB_resPath, filepath);

                filepath = AB_resPath + "/" + filepath;
                Debug.Log("1:" + filepath);
            }
            else if (ABrespathlist.ContainsKey(filepath))
            {
                filepath = ABrespathlist[filepath];
                Debug.Log("2:" + filepath);
            }



            foreach (string guid in itemsGuid)
            {
                ////获取没有/的地址文件名字
                ///替换方法 Path.GetFileNameWithoutExtension 获取一个路径下不包含扩展名的文件名字
                //string tempstr = AssetDatabase.GUIDToAssetPath(guid);
                //for (int i = AssetDatabase.GUIDToAssetPath(guid).Length; i > 1; i--)
                //{
                //    tempstr = tempstr.Remove(0, tempstr.IndexOf('/') + 1);
                //    //Debug.Log("最终剪辑地址:" + tempstr);
                //}
                DirectoryInfo direction = new DirectoryInfo(AssetDatabase.GUIDToAssetPath(guid));
                if (direction.Exists)
                {
                    FileInfo[] files = direction.GetFiles();
                    foreach (FileInfo item in files)
                    {
                        //Debug.Log(AssetDatabase.GUIDToAssetPath(guid) + "/" + item.Name + " 新的地址是 : " + Path.Combine(filepath + "/" + item.Name));
                        File.Move(AssetDatabase.GUIDToAssetPath(guid) + "/" + item.Name, Path.Combine(filepath + "/" + item.Name));
                    }
                    //AssetDatabase.MoveAsset(AssetDatabase.GUIDToAssetPath(guid), filepath + "/" + Path.GetFileName(AssetDatabase.GUIDToAssetPath(guid)));
                    //File.Delete(AssetDatabase.GUIDToAssetPath(guid));
                }
                else
                {
                    //Debug.LogError(AssetDatabase.GUIDToAssetPath(guid) + " ====== " + filepath + "/" + Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(guid)));
                    //File.Move(AssetDatabase.GUIDToAssetPath(guid), filepath + "/" + Path.GetFileName(AssetDatabase.GUIDToAssetPath(guid)));
                    AssetDatabase.MoveAsset(AssetDatabase.GUIDToAssetPath(guid), filepath + "/" + Path.GetFileName(AssetDatabase.GUIDToAssetPath(guid)));
                }
            }
            //Debug.LogError(AssetDatabase.MoveAsset(AssetDatabase.GUIDToAssetPath(guid), "Assets/AB_res/" + tempstr));
            AssetDatabase.Refresh();
            GetWindow<MySimpleEditorWindow>().Close();
        }

        private void setlael()
        {
            label = currentlabel;
        }
        private void setgroupname()
        {
            GroupName = currentgroup;
        }
        private void setpath()
        {
            filepath = currentpath;
        }


        [MenuItem("Example/FindAssets Example")]
        static void ExampleScript()
        {
            //AssetDatabase.FindAssets();//只是获取名字是某某或类似的文件的guid 不是获取子类的所有guid
            // Find all assets labelled with 'architecture' :
            //string[] guids1 = AssetDatabase.FindAssets("AB_res");
            //foreach (string guid1 in guids1)
            //{
            //    Debug.Log(guid1 + ":guid1");
            //    Debug.Log(AssetDatabase.GUIDToAssetPath(guid1));
            //    //AssetDatabase.Contains("BGChangJing");
            //}

            AssetDatabase.MoveAsset("Assets/ya", "Assets/AB_res/ya");
            //Material material = new Material(Shader.Find("Specular"));
            //AssetDatabase.CreateAsset(material, "Assets/MyMaterial.mat");

            // Print the path of the created asset
            //Debug.Log(AssetDatabase.GetAssetPath(material));

            // Find all Texture2Ds that have 'co' in their filename, that are labelled with 'architecture' and are placed in 'MyAwesomeProps' folder
            //string[] guids2 = AssetDatabase.FindAssets("co l:architecture t:texture2D", new[] { "Assets/MyAwesomeProps" });

            //foreach (string guid2 in guids2)
            //{
            //    Debug.Log(AssetDatabase.GUIDToAssetPath(guid2));
            //}
        }

        [MenuItem("Assets/AddressAbleEditorWindow", priority = 0)]
        private static void OpenWindow()
        {
            //List<string> listnames = new List<string>() { "1", "2" };
            //ABsetting.GetLabels();
            //listname


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

            List<string> listGroupstr = new List<string>();

            foreach (var item in ABsetting.groups)
            {
                listGroupstr.Add(item.name);
            }

            MySimpleEditorWindow mySimple = GetWindow<MySimpleEditorWindow>();
            mySimple.listGroupname = listGroupstr;
            mySimple.listlabel = ABsetting.GetLabels();
            mySimple.folder = Selection.activeObject.name;
            mySimple.itemsGuid = Selection.assetGUIDs;
            //默认创建的名字是所选的file 名字
            mySimple.filepath = Selection.activeObject.name;

            mySimple.instanceid = Selection.instanceIDs;

            //foreach (var item in mySimple.instance)
            //{
            //    Debug.Log(AssetDatabase.GetAssetPath(item));//获取相对assets的路径
            //}

            //foreach (var item in Selection.assetGUIDs)
            //{
            //    //获取相对assets的路径
            //    Debug.Log(AssetDatabase.AssetPathToGUID(item));
            //}
            //Debug.Log(Path.Combine(Application.dataPath + Application.streamingAssetsPath));

            //mySimple.newpathlist = ;
            DirectoryInfo direction = new DirectoryInfo("Assets/AB_res");
            //获取文件夹，exportPath是文件夹的路径
            //Debug.LogError(newpathlist);

            //只是获取到第一层
            DirectoryInfo[] files = direction.GetDirectories("*", SearchOption.AllDirectories);
            Debug.LogError(files.Length + ":长度");
            foreach (DirectoryInfo item in files)
            {
                mySimple.newpathlist.Add(item.FullName.Remove(0, item.FullName.IndexOf("Assets")).Replace('\\', '/'));
                //Debug.Log(item.FullName.Remove(0, item.FullName.IndexOf("Assets")));

                mySimple.ABrespathlist.Add(item.Name, item.FullName.Remove(0, item.FullName.IndexOf("Assets")).Replace('\\', '/'));
                //mySimple.newfullpathlist.Add(item.FullName);
            }
        }
    }
}
