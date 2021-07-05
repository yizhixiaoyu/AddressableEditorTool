using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

[Serializable]
public class ResourcesLoadManage : monoSingleton<ResourcesLoadManage>
{
    //private Dictionary<AsyncOperationHandle, Action<AsyncOperationHandle>> asyncUiDict = new Dictionary<AsyncOperationHandle, Action<AsyncOperationHandle>>();
    private List<AsyncOperationHandle> asyncList = new List<AsyncOperationHandle>();

    private static List<Sprite> Allsprite = new List<Sprite>();
    private static List<GameObject> Allgameobject = new List<GameObject>();
    private static List<AudioClip> Allaudioclip = new List<AudioClip>();
    private static List<VideoClip> AllVideoClips = new List<VideoClip>();

    //private static string SpriteLabel = "Sprite";
    //private static string GameobjectLabel = "Prefab";
    private static string AudioclipLabel = "Audio";
    //private static string VideoclipLabel = "Video";
    private Slider slider;
    private Text sliderText;

    /// <summary>
    /// 正式和测试
    /// </summary>
    public enum Typeget
    {
        TEST,
        RELEASE
    }
    public Typeget initType = Typeget.RELEASE;
    public void Init(bool isRelease)
    {
        initType = isRelease ? Typeget.RELEASE : Typeget.TEST;
        if (initType == Typeget.RELEASE)
        {
            slider = GameObject.Find("Canvas/Slider").GetComponent<Slider>();
            sliderText = GameObject.Find("Canvas/Text").GetComponent<Text>();
        }
        //LoadAssetReource<GameObject>(GameobjectLabel, losadGameobject);
        //如果没有含有video的标签
        //LoadAssetReource<VideoClip>(VideoclipLabel, losadVideo);
        LoadAssetReource<AudioClip>(AudioclipLabel, losadAudio);
        //LoadAssetReource<Sprite>(SpriteLabel, losadSprite);

        //AsyncOperationHandle<IList<IResourceLocation>> async = Addressables.LoadResourceLocationsAsync(VideoclipLabel, typeof(Sprite));
        //Debug.Log(async.Result.Count + $"VideoclipLabel :标签的数量");
        //foreach (IResourceLocation item in async.Result)
        //{
        //}

        //加载在内存中的程序集列表
        //foreach (Assembly item in AppDomain.CurrentDomain.GetAssemblies())
        //{
        //    Debug.LogError(item.Location);
        //    Debug.LogError(item.CodeBase);
        //    Debug.LogError(item.GetName().Name);
        //}
    }
    private void LoadAssetReource<T>(string key, Action<T> action)
    {
        asyncList.Add(Addressables.LoadAssetsAsync<T>(key, action));
    }
    private void losadAudio(AudioClip obj)
    {
        Allaudioclip.Add(obj);
        Debug.LogError("长度和第一个的名字" + Allaudioclip.Count + "" + Allaudioclip[0].name);
    }

    private void losadVideo(VideoClip obj)
    {
        AllVideoClips.Add(obj);
    }

    private void losadGameobject(GameObject obj)
    {
        Allgameobject.Add(obj);
    }
    private void losadSprite(Sprite obj)
    {
        Allsprite.Add(obj);
    }

    float Asycindex = 0;
    bool istrue = false;
    private float slidervalue;
    private float tempvalue;
    //private float Timetemp;
    //private float TimeTiao;
    private void Update()
    {
        if (!istrue)
        {
            Asycindex = 0;
            slidervalue = 0;
            foreach (var item in asyncList)
            {
                slidervalue += item.PercentComplete;
                if (item.IsDone)
                {
                    Asycindex++;
                    if (Asycindex == asyncList.Count)
                    {
                        slidervalue = asyncList.Count;
                    }
                }
            }
            //Debug.LogError(slidervalue);
            if (tempvalue * asyncList.Count < slidervalue)
            {
                tempvalue = slidervalue / asyncList.Count;

            }
            if (initType == Typeget.RELEASE)
            {
                slider.value = Mathf.Lerp(slider.value, tempvalue, Time.deltaTime);
                sliderText.text = "进度" + (int)(slider.value * 1000 + 1) * 0.1 + "%";
                if (Mathf.Abs(slider.value - slider.maxValue) < 0.01f)
                {
                    istrue = true;
                    slider.value = slider.maxValue;
                    SceneManager.LoadScene(1);
                    Debug.LogError("资源加载已经完成了");
                }
            }
        }
    }
    /// <summary>
    /// 所有的精灵数据
    /// </summary>
    public List<Sprite> Allsprites
    {
        get { return Allsprite; }
    }
    /// <summary>
    /// 所有的gameobject格式数据
    /// </summary>
    public List<GameObject> Allgames
    {
        get { return Allgameobject; }
    }
    /// <summary>
    /// 所有的视频数据
    /// </summary>
    public List<VideoClip> Allvideos
    {
        get { return AllVideoClips; }
    }
    /// <summary>
    /// 获取所有声音数据
    /// </summary>
    public List<AudioClip> Allaudios
    {
        get { return Allaudioclip; }
    }
}







