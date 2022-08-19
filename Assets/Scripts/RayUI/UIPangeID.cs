using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 此脚本负责UI页面的导出，我们先学习如何使用该脚本，暂不涉及其实现
/// 用法：首先我们需要提供一个代码生成模版
/// </summary>
public class UIPangeID : MonoBehaviour
{
    #region 定义界面属性

    public UIType uitype = UIType.Normal;
    public UIMode uimode = UIMode.DoNothing;
    public UICollider uicollider = UICollider.None;

    #endregion


    private string SCRIPT_GEN_PATH = "Assets/Scripts/Myui";
    private string SCRIPT_TEMPLATE_PATH = "Assets/Plugins/RayUI/Template";//代码模板路径
    //public static string UI_ROOT_PATH = "UI"; // NB: Addressable不需要根路径，把UI资源做一下名称简化保持【资源名==根对象名】即可

    public class UIFieldInfo
    {
        #region declaration
        public string fieldType;
        public string fieldName;
        #endregion

        #region initialization
        public string fieldPath;
        #endregion

        public override string ToString()
        {
            return $"{fieldType} {fieldName} => {fieldPath}";
        }
    }

    private static string GetFieldType(WidgetID w)
    {
        MonoBehaviour mono;
        if ((mono = w.GetComponents<WidgetID>().FirstOrDefault(x => x != w)) != null)
        {
            return mono.GetType().Name;
        }
        else if ((mono = w.GetComponent<Selectable>()) != null)
        {
            return mono.GetType().Name;
        }
        else if ((mono = w.GetComponent<Graphic>()) != null)
        {
            return mono.GetType().Name;
        }
        else
        {
            return "Transform";
        }
    }

    public static void _Gen(Transform tr, string path, List<UIFieldInfo> list)
    {
        if (tr == null)
            return;



        foreach (Transform c in tr)
        {
            string cPath = path == string.Empty ? c.name : path + "/" + c.name;


            var widgets = c.GetComponents<WidgetID>();
            foreach (var w in widgets)
            {
                if (w && w.GetType() == typeof(WidgetID) && w.ignore == false)
                {
                    //Debug.Log("+" + cPath);
                    string fname = c.name.InitialLower();
                    var fieldInfo = list.Find(x => x.fieldName == fname);
                    if (fieldInfo != null)
                        fname = "_" + c.FullPath();
                    list.Add(new UIFieldInfo()
                    {
                        fieldName = fname.Replace("/", string.Empty),
                        fieldPath = cPath,
                        fieldType = GetFieldType(w),
                    });
                    break;
                }
            }
            //cPath = cPath.Trim();

            _Gen(c, cPath, list);
        }
    }

    //[MenuItem("GameObject/RayGame/生成UIPage脚本", priority = 0)]

    /// <summary>
    /// 注意::: 需要获取的组件挂WidgetID,获取的组件gameobject名字 
    /// 不要用系统默认的名字 不然会把路径当做组件名字(很长) 
    /// </summary>
    [ContextMenu("生成UIPage脚本")]
    public void Gen()
    {
        string UIPAGE_CLASS_DEF = File.ReadAllText(SCRIPT_TEMPLATE_PATH + "/UIPageTemplateS1.cs.txt", Encoding.UTF8);

        string VIEW_CLASS_DEF = File.ReadAllText(SCRIPT_TEMPLATE_PATH + "/UIViewTemplateS1.cs.txt", Encoding.UTF8);


        var fieldList = new List<UIFieldInfo>();
        _Gen(Selection.activeTransform, string.Empty, fieldList);

        StringBuilder sbFieldDef = new StringBuilder();
        StringBuilder sbFieldInit = new StringBuilder();
        foreach (var field in fieldList)
        {
            sbFieldDef.AppendLine($"\tpublic {field.fieldType} {field.fieldName};");
            sbFieldInit.AppendLine($"\t\t{field.fieldName} = transform.Find(\"{field.fieldPath}\").GetComponent<{field.fieldType}>();");
        }
        string sPage = UIPAGE_CLASS_DEF
            .Replace("{ROOT_UI_NAME}", Selection.activeGameObject.name)
            .Replace("{UI_WIDGET_FIELD_LIST}", sbFieldDef.ToString())
            .Replace("{FIELD_INITIALIZATION_LIST}", sbFieldInit.ToString())
            .Replace("{UI_PATH}", /*UI_ROOT_PATH + "/" +*/ Selection.activeGameObject.name);



        string sView = VIEW_CLASS_DEF
            .Replace("{ROOT_UI_NAME}", Selection.activeGameObject.name)
            .Replace("{UI_TYPE}", uitype.ToString())
            .Replace("{UI_MODE}", uimode.ToString())
            .Replace("{UI_COLLIDER}", uicollider.ToString());

        string scriptPath;
        if (Selection.activeGameObject.name.IndexOf("Page") > 0)
        {
            scriptPath = SCRIPT_GEN_PATH + "/" + Selection.activeGameObject.name + ".cs";
        }
        else
        {
            scriptPath = SCRIPT_GEN_PATH + "/" + Selection.activeGameObject.name + "Page.cs";
        }

        if (File.Exists(scriptPath))
            File.Delete(scriptPath);
        Debug.Log(scriptPath + "   ," + sPage);
        File.WriteAllText(scriptPath, sPage, Encoding.UTF8);

        string viewPath;

        if (Selection.activeGameObject.name.IndexOf("Page") > 0)
        {
            viewPath = SCRIPT_GEN_PATH + "/" + Selection.activeGameObject.name.Replace("Page", "View") + ".cs";
        }
        else
        {
            viewPath = SCRIPT_GEN_PATH + "/" + Selection.activeGameObject.name + "View" + ".cs";
            //Debug.LogError("不存在");
        }

        // NB: 视图文件不能自动删除并重建，因为可能已经写了很多代码了
        if (File.Exists(viewPath) == false
            || (EditorUtility.DisplayDialog("文件已存在，是否覆盖？", $"File Name: {viewPath}", "是", "否")
            && EditorUtility.DisplayDialog("文件已存在，是否覆盖？", $"File Name: {viewPath}", "是", "否"))
            )
        {
            Debug.Log(sView);
            File.WriteAllText(viewPath, sView, Encoding.UTF8);
        }

        Debug.Log(uitype.ToString());
        //Debug.LogError("是否为空");
        AssetDatabase.Refresh();
        Debug.Log("已经完成了");
    }
}
