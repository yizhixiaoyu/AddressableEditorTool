//=====================================================
// - FileName:      resload.cs
// - Created:       #CreateTime# 
// - UserName:      #AuthorName#
// - Email:         #AuthorEmail#
// - Description:   
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class resload
{
    /// <summary>
    /// 查找对应的gamgobject
    /// </summary>
    /// <param name="gamename"></param>
    /// <returns></returns>
    public static GameObject select(this string gamename)
    {
        //leftUI = ResourcesLoadManage.instance.Mygame.Where(u => u.name == "malfunctionItem").First();
        foreach (GameObject item in ResourcesLoadManage.Instance.Allgames)
        {
            //Debug.LogError(item.name + "  allgameobjct的长度为 +" + ResourcesLoadManage.instance.Mygame.Count);
            if (item.name == gamename)
            {
                return item;
            }
        }

        Debug.LogError("没有名字为" + gamename + "的物品,请检查addressable groups");
        return null;
    }

    public static Sprite selectSprite(this string gamename)
    {
        //leftUI = ResourcesLoadManage.instance.Mygame.Where(u => u.name == "malfunctionItem").First();
        foreach (Sprite item in ResourcesLoadManage.Instance.Allsprites)
        {
            //Debug.LogError(item.name + "  allgameobjct的长度为 +" + ResourcesLoadManage.instance.Mygame.Count);
            if (item.name == gamename)
            {
                return item;
            }
        }

        Debug.LogError("没有名字为" + gamename + "的物品,请检查addressable groups");
        return null;
    }
    public static AudioClip selectAudio(string audo)
    {
        foreach (AudioClip item in ResourcesLoadManage.Instance.Allaudios)
        {
            if (item.name == audo)
            {
                return item;
            }
            Debug.LogError(item.name+" :音乐名字为");
        }
        Debug.LogError("没有名字为" + audo + "的audioclip,请检查Audio的addressable groups");
        return null;
    }
}