//=====================================================
// - FileName:      Init.cs
// - Created:       #CreateTime# 
// - UserName:      #AuthorName#
// - Email:         #AuthorEmail#
// - Description:   
//======================================================
using UnityEngine;

public class Init : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        ResourcesLoadManage.Instance.Init(true);
    }
}