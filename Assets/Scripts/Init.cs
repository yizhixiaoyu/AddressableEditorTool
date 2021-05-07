//=====================================================
// - FileName:      Init.cs
// - Created:       #CreateTime# 
// - UserName:      #AuthorName#
// - Email:         #AuthorEmail#
// - Description:   
//======================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour
{
    /// <summary>
    ///初始化资源加载
    /// </summary>
    void Start()
    {
        ResourcesLoadManage.Instance.Init(true);
    }
}