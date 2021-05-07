//=====================================================
// - FileName:      HOuse.cs
// - Created:       #CreateTime# 
// - UserName:      #AuthorName#
// - Email:         #AuthorEmail#
// - Description:   
//======================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class House : MonoBehaviour
{
    public enum floornumEnum
    {
        F2 = 2,
        F3 = 3,
        F4 = 4,
        F5 = 5,
        F6 = 6,
        F7 = 7,
        F8 = 8,
        F9 = 9,
        F10 = 10,
        F11 = 11,
        F12 = 12,
        F13 = 13,
        F14 = 14,
        F15 = 15,
        F16 = 16,
        F17 = 17,
        F18 = 18,
    }

    public floornumEnum floorNum;
    private Dictionary<int, float> floorDict = new Dictionary<int, float>();

    private bool isTwoCar;
    public Text AllPrice;
    public Text Threeprice;
    public Text Twoprice;
    private Dropdown Dropdown;
    public Text alltext;
    public int m = 119;
    private List<int> floornumber = new List<int>()
    {
        4900,5200,5300,5460,5470,5480,5490,5500,5500,5500,5500,5500,5500,5500,5500,5500,5000
    };
    private void Start()
    {
        Dropdown = transform.Find("Dropdown").GetComponent<Dropdown>();
        foreach (string item in floornumEnum.GetNames(typeof(floornumEnum)))
        {
            Dropdown.options.Add(new Dropdown.OptionData(item));


        }
        Dropdown.captionText.text = floornumEnum.GetNames(typeof(floornumEnum))[0];
        for (int i = 0; i < floornumber.Count; i++)
        {
            floorDict.Add(i + 2, floornumber[i] / 10000.0f);
            //Debug.LogError("是否一样:" + floorDict[i + 2] + " 添加价格:" + floornumber[i]);
        }
        //默认第一个
        //Setprice((floornumEnum)Enum.ToObject(typeof(floornumEnum), 2));
        Dropdown.onValueChanged.AddListener((int index) =>
        {
            //(floornumEnum)Enum.ToObject(typeof(floornumEnum), index)//根据数值转换Enum
            Setprice((floornumEnum)Enum.ToObject(typeof(floornumEnum), (index + 2)));
        });
        foreach (string item in floornumEnum.GetNames(typeof(floornumEnum)))
        {
            Setprice(GetEnum<floornumEnum>(item));
        }

        //foreach (var item in floornumEnum.GetValues(typeof(floornumEnum)))
        //{
        //    var attrs = item.GetType().GetField(item.ToString()).GetCustomAttributes(typeof(floornumEnum), true);
        //    //Debug.LogError(attrs);
        //    foreach (var attri in attrs)
        //    {
        //        Debug.LogError("特性");
        //        Debug.LogError(attri);
        //    }
        //}
    }

    /// <summary>
    /// 把一个枚举变成字典来用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Dictionary<int, string> EnumToDictionary<T>()
    {
        Dictionary<int, string> dic = new Dictionary<int, string>();
        if (!typeof(T).IsEnum)
        {
            return dic;
        }
        string desc = string.Empty;
        foreach (var item in Enum.GetValues(typeof(T)))
        {
            var attrs = item.GetType().GetField(item.ToString()).GetCustomAttributes(typeof(T), true);
            //if (attrs != null && attrs.Length > 0)
            //{
            //    DescriptionAttribute descAttr = attrs[0] as DescriptionAttribute;
            //    desc = descAttr.Description;
            //}
            dic.Add(Convert.ToInt32(item), desc);
        }
        return dic;
    }
    /// <summary>
    /// 根据传入的int返回对应枚举属性名称
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="num">进制</param>
    /// <returns> Enum.GetName(typeof(Colors), Colors.Blue))</returns>
    public static string GetEnumName<T>(int value)
    {
        string name = "";
        //name = Enum.Parse(typeof(T), Enum.GetName(typeof(T), value)).ToString();
        //if (Enum.GetName(typeof(T), value) == name)
        //{
        //    Debug.LogError("名字是:" + name);
        //}
        name = Enum.GetName(typeof(T), value);
        return name;
    }
    /// <summary>
    /// 根据int值转换为Enum的值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">enum对应的value值</param>
    /// <returns></returns>
    public static T GetEnum<T>(int value)
    {
        return (T)Enum.ToObject(typeof(T), value);
    }

    /// <summary>
    /// 根据string值转换为Enum值
    /// </summary>
    /// <typeparam name="T">enum的Type</typeparam>
    /// <param name="value">Enum对应的字符值</param>
    /// <returns></returns>
    public static T GetEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value);
    }
    /// <summary>
    /// 根据传入的枚举属性获得对应值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int GetEnumValue<T>(string value)
    {
        Type type = typeof(T);
        var schoolId = Enum.Format(type, Enum.Parse(type, value.ToUpper()), "d");
        return Convert.ToInt32(schoolId);
    }

    public void Setprice(floornumEnum floorNum)
    {
        isTwoCar = false;
        switch (floorNum)
        {
            case floornumEnum.F2:
                pricecount(floorDict[floornumEnum.F2.GetHashCode()], 2);
                Debug.LogError(floorDict[floornumEnum.F2.GetHashCode()] + "价格" + floornumEnum.F2.GetHashCode());
                break;
            case floornumEnum.F3:
                pricecount(floorDict[floornumEnum.F3.GetHashCode()], 3);
                break;
            case floornumEnum.F4:
                pricecount(floorDict[floornumEnum.F4.GetHashCode()], 4);
                break;
            case floornumEnum.F5:
                pricecount(floorDict[floornumEnum.F5.GetHashCode()], 5);
                break;
            case floornumEnum.F6:
                pricecount(floorDict[floornumEnum.F6.GetHashCode()], 6);
                break;
            case floornumEnum.F7:
                pricecount(floorDict[floornumEnum.F7.GetHashCode()], 7);
                break;
            case floornumEnum.F8:
                pricecount(floorDict[floornumEnum.F8.GetHashCode()], 8);
                break;
            case floornumEnum.F9:
                pricecount(floorDict[floornumEnum.F9.GetHashCode()], 9);
                break;
            case floornumEnum.F10:
                pricecount(floorDict[floornumEnum.F10.GetHashCode()], 10);
                break;
            case floornumEnum.F11:
                isTwoCar = true;
                pricecount(floorDict[floornumEnum.F11.GetHashCode()], 11);
                break;
            case floornumEnum.F12:
                isTwoCar = true;
                pricecount(floorDict[floornumEnum.F12.GetHashCode()], 12);
                break;
            case floornumEnum.F13:
                pricecount(floorDict[floornumEnum.F13.GetHashCode()], 13);
                break;
            case floornumEnum.F14:
                pricecount(floorDict[floornumEnum.F14.GetHashCode()], 14);
                break;
            case floornumEnum.F15:
                isTwoCar = true;
                pricecount(floorDict[floornumEnum.F15.GetHashCode()], 15);
                break;
            case floornumEnum.F16:
                isTwoCar = true;
                pricecount(floorDict[floornumEnum.F16.GetHashCode()], 16);
                break;
            case floornumEnum.F17:
                pricecount(floorDict[floornumEnum.F17.GetHashCode()], 17);
                break;
            case floornumEnum.F18:
                pricecount(floorDict[floornumEnum.F18.GetHashCode()], 18);
                break;
            default:
                break;
        }
    }
    //11 12 15 16 双楼层 
    /// <summary>
    /// 计算价格
    /// </summary>
    public int carmin = 10;
    public int carmax = 12;
    public int boxroommin = 2;
    public int boxroommax = 5;

    private void pricecount(float price, int flnum)
    {

        Debug.LogError(price + ":价格是多少");
        if (isTwoCar)
        {
            AllPrice.text = "总价价格:" + (price * m + carmin * 2 - 3 + boxroommin) + "=>" + (price * m + carmax * 2 - 3 + boxroommax);
            Threeprice.text = "百分之30价格:" + ((int)((price * m * 0.3 + carmin * 2 - 3 + boxroommin) * 10000)) / 10000.0f + "W" + "=>" + ((int)((price * m * 0.3 + carmax * 2 - 3 + boxroommax) * 10000)) / 10000.0f + "W";
            Twoprice.text = "百分之20价格:" + ((int)((price * m * 0.2 + carmin * 2 - 3 + boxroommin) * 10000)) / 10000.0f + "W" + "=>" + ((int)((price * m * 0.2 + carmax * 2 - 3 + boxroommax) * 10000)) / 10000.0f + "-------(双车位)";
        }
        else
        {
            AllPrice.text = "总价价格:" + (price * m + carmin + 2) + "W" + "=>" + (price * m + carmax + 5) + "W";
            Threeprice.text = "百分之30价格:" + ((int)((price * m * 0.3 + carmin + 2) * 10000)) / 10000.0f + "W" + "=>" + ((int)((price * m * 0.3 + carmax + 5) * 10000)) / 10000.0f;
            Twoprice.text = "百分之20价格:" + ((int)((price * m * 0.2 + carmin + 2) * 10000)) / 10000.0f + "W" + "=>" + ((int)((price * m * 0.2 + carmax + 5) * 10000)) / 10000.0f;
        }
        alltext.text += (flnum + "楼层:" + "单价" + price + "W" + "房子价格" + price * m + AllPrice.text + ":::" + Threeprice.text + ":::" + Twoprice.text + "\r");
    }
}