using System.IO;
using UnityEditor;

public static class AssistentHelper
{
    public static void CheckDirectory(string dir)
    {
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }

    public static void CheckFile(string path)
    {
        if (!File.Exists(path))
        {
            FileStream fs = File.Create(path);
            fs.Close();
            AssetDatabase.Refresh();
        }
    }
}
