//=====================================================
// - FileName:      ExampleTest.cs
// - Created:       #CreateTime# 
// - UserName:      #AuthorName#
// - Email:         #AuthorEmail#
// - Description:   
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleTest : MonoBehaviour
{
    private AudioSource audo;
    public AudioClip clip;
    void Start()
    {
        audo = GetComponent<AudioSource>();
        audo.clip = resload.selectAudio("BGChangJing");
        audo.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}