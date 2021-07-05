using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 自定义生成列表
/// </summary>
public class CustomToolModul : EditorWindow
{
    [MenuItem("Tools/脚本自动生成")]
    static public void ShowWindow()
    {
        EditorWindow window = EditorWindow.GetWindow<CustomToolModul>();
        window.minSize = new Vector2(400, 450);
        window.maxSize = new Vector2(400, 450);
        window.Show();
    }
    private string configName;

    /// <summary>
    /// 生成的配置文件脚本路径
    /// </summary>
    public const string SCIRIPTPATH = "Scripts/ToolModul/Config/Scripts/ConfigScripts";

    /// <summary>
    /// 配置文件Type生成路径
    /// </summary>
    public const string CONFIGTYPEPATH = "Scripts/ToolModul/Config/Scripts/ConfigType.cs";

    public void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        configName = EditorGUILayout.TextField("配置文件名称", configName, GUILayout.MaxWidth(400));
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("确定生成配置文件"))
        {
            Debug.Log($"当前生成的配置文件名称为:{configName}");
            if (!string.IsNullOrEmpty(configName))
            {
                string scriptPath = $"{Application.dataPath}/{SCIRIPTPATH}";
                string configTypePath = $"{Application.dataPath}/{CONFIGTYPEPATH}";
                AssistentHelper.CheckDirectory(scriptPath);
                //  AssistentHelper.CheckDirectory(configTypePath);
                scriptPath = $"{scriptPath}/{configName}.cs";
                if (!File.Exists(scriptPath))
                {
                    Debug.Log("开始创建");
                    using (FileStream fs = new FileStream(scriptPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                        {
                            string code = ScriptTemplate.ConfigScript;
                            code = code.Replace("类名", configName);
                            sw.Write(code);
                        }
                    }

                    bool needWriteConfigType = true;
                    string configType = File.ReadAllText(configTypePath);
                    string pattern = "public const string ([A-Za-z0-9_]+) = \"([A-Za-z0-9_]+)\"";
                    MatchCollection matchs = Regex.Matches(configType, pattern);

                    for (int i = 0; i < matchs.Count; i++)
                    {
                        string mathName = matchs[i].Groups[1].Value;
                        if (mathName == configName)
                        {
                            needWriteConfigType = false;
                        }
                    }
                    Debug.Log(needWriteConfigType);
                    if (!needWriteConfigType) return;
                    using (FileStream fs = new FileStream(configTypePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                        {
                            StringBuilder sb = new StringBuilder();

                            for (int i = 0; i < matchs.Count; i++)
                            {
                                string matchName = matchs[i].Groups[1].Value;
                                sb.Append($"public const string {matchName} = \"{matchName}\";\r\n\t\t");
                            }
                            sb.Append($"public const string {configName} = \"{configName}\";");

                            // 写入
                            string code = ScriptTemplate.ConfigType;
                            code = code.Replace("面板", sb.ToString());
                            sw.Write(code);
                        }
                    }
                    AssetDatabase.Refresh();

                }
            }
        }
    }
}
