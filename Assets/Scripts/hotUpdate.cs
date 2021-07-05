//=====================================================
// - FileName:      hotUpdate.cs
// - Created:       #CreateTime# 
// - UserName:      #AuthorName#
// - Email:         #AuthorEmail#
// - Description:   
//======================================================
using UnityEngine;
using UnityEngine.UI;

public class hotUpdate : MonoBehaviour
{

    public Image bgImage;
    void Start()
    {
        bgImage.sprite = resload.selectSprite("bg");
    }

    // Update is called once per frame
    void Update()
    {

    }
}