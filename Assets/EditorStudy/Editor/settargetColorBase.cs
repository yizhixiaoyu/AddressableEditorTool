using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(setColot))]
public class settargetColorBase : Editor
{
    private void OnSceneGUI()
    {
        setColot obj = (setColot)target;//挂载setcolor脚本的目标物体

        Handles.Label(obj.transform.position + Vector3.up, obj.name + ":" + obj.transform.position.ToString());
        Handles.BeginGUI();//标头 开始绘制Gui

        GUILayout.BeginArea(new Rect(0, 50, 200, 200));//2d区域坐标和区域
        GUI.color = Color.red;
        GUILayout.Label("选择颜色");
        GUI.color = Color.red;

        if (GUILayout.Button("红色"))
        {
            obj.GetComponent<Renderer>().sharedMaterial.color = Color.red;
        } 
        GUI.color = Color.green;

        if (GUILayout.Button("绿色"))
        {
            obj.GetComponent<Renderer>().sharedMaterial.color = Color.green;
        }
         GUI.color = Color.blue;

        if (GUILayout.Button("蓝色"))
        {
            obj.GetComponent<Renderer>().sharedMaterial.color = Color.blue;
        }


        //Handles.Label(obj.transform.position + Vector3.up, obj.name + ":" + obj.transform.position.ToString());
        Handles.EndGUI();
        GUILayout.EndArea();
    }

}
