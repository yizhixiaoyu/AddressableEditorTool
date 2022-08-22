using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class Utils
{
    /* 
    #region 定点数支持
    /// <summary>
    /// 浮点数转定点数
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    public static LFloat ToFLoat(this float o)
    {
        return new LFloat(true, (long)(o * LFloat.PrecisionFactor));
    }
    public static LVector3 ToLVector3(this Vector3 o)
    {
        return new LVector3(
            o.x.ToLFloat(),
             o.y.ToLFloat(),
              o.z.ToLFloat()
            );
    }

    /// <summary>
    /// LVector3转unity中的Vector3
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    public static Vector3 ToVector3(this LVector3 o)
    {
        return new Vector3(
            o.x.ToFloat(),
             o.y.ToFloat(),
              o.z.ToFloat()
            );
    }

    /// <summary>
    /// LQuaternion转unity中的Quaternion
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    public static Quaternion ToQuaternion(this LQuaternion o)
    {
        return new Quaternion(
            o.x.ToFloat(),
             o.y.ToFloat(),
              o.z.ToFloat(),
               o.w.ToFloat()
            );
    }
    public static LQuaternion ToLQuaternion(this Quaternion o)
    {
        return new LQuaternion(
            o.x.ToLFloat(),
             o.y.ToLFloat(),
              o.z.ToLFloat(),
               o.w.ToLFloat()
            );
    }
   */
    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="sep"></param>
    /// <returns></returns>
    public static string ToStringEx(this object obj, string sep = ",")
    {
        //获取对象类型
        Type type = obj.GetType();

        //判断该类型是否可以转换为IList接口,数组和 list都实现了IList这个接口 所以这种方式比较巧妙,同时判定了数组和list的情况

        if (typeof(IList).IsAssignableFrom(type))
        {
            //用字符串池连接格式化后的字符串,减少gc占用
            StringBuilder sb2 = new StringBuilder();

            var list = obj.As<IList>();//把对象转换成IList接口

            if (list.Count > 1)
            {
                sb2.AppendLine($"[{list.Count}]");
                //list中如果有超过1个元素 则用方法号 包裹元素数量
            }
            else
            {
                sb2.Append($"[{list.Count}]==>");
                //只有1个 或8 个就用剪头指向元素值
            }
            //遍历每一个对象
            for (int i2 = 0; i2 < list.Count; i2++)
            {
                var elem = list[i2];
                sb2.Append(elem.ToStringEx(sep) + (i2 == list.Count - 1 ? "" : "\r\n"));//递归调用本方法来取得字符串

            }
            return sb2.ToString();//递归的层次返回

        }
        else
        {
            //如果对象不是数组类型  则逐个输出该对象的成员
            StringBuilder sb = new StringBuilder();

            var fis = type.GetFields();

            for (int i = 0; i < fis.Length; i++)//逐个遍历每个字段
            {
                FieldInfo fi = fis[i];

                object v = fi.GetValue(obj);//取得字段值

                if (v.GetType().IsGenericType == false)
                {//如果是应用类型(也就是非简单的数值类型)
                    v = v.ToStringEx(sep);//则进一步递归输出该字段对象的每一个成员,由于TostringEx返回string,
                                          //所以v的内容 实际上就是字符串了吗如果是简单类型
                                          //那么下面appendformat 时 C# 会自动在要求字符串参数的函数中把对象调用TOstring 转成字符串了
                }
                sb.AppendFormat(
                    "{0}={1}{2}",
                    fi.Name,
                    v, //v.tostring
                    i == fis.Length - 1 ? "" : sep
                    );

            }
            return sb.ToString();//递归的层次返回
        }
    }


    //#endregion
    public static T As<T>(this object o) { return (T)o; }

    public static T DeepClone<T>(this T o)
    {
        string js = JsonMapper.ToJson(o);
        return JsonMapper.ToObject<T>(js);

        //这里来回转换一下 就会在内部穿讲个一个新的对象,并将字符串内存存储的对象信息完全拷贝到新建对象中 这样完成了深拷贝;;;;

    }

    public static string InitialLower(this string s)
    {
        StringBuilder sb = new StringBuilder(s);
        sb[0] = char.ToLower(sb[0]);
        return sb.ToString();
    }
    public static string InitialUpper(this string s)
    {
        StringBuilder sb = new StringBuilder(s);
        sb[0] = char.ToUpper(sb[0]);
        return sb.ToString();
    }
    /// <summary>
    /// 获取tr 在hierarchy中所代表的的节点完成路径
    /// </summary>
    /// <param name="tr"></param>
    /// <returns></returns>
    public static string FullPath(this Transform tr)
    {
        string s = "";
        do
        {
            tr = tr.parent;//逐级 往父节点搜寻
            if (tr == null)//层次节点没有父节点
                break;
            s = tr.gameObject.name+"/"+s;//
        } while (true);
        return s;
    }

    // find out if any input is currently active by using Selectable.all
    // (FindObjectsOfType<InputField>() is far too slow for huge scenes)
    public static bool AnyInputActive()
    {
        return Selectable.allSelectables.Any(
            sel => sel is InputField && ((InputField)sel).isFocused
        );
    }

    // check if the cursor is over a UI or OnGUI element right now
    // note: for UI, this only works if the UI's CanvasGroup blocks Raycasts
    // note: for OnGUI: hotControl is only set while clicking, not while zooming
    public static bool IsCursorOverUserInterface()
    {
        // IsPointerOverGameObject check for left mouse (default)
        if (EventSystem.current.IsPointerOverGameObject())
            return true;

        // IsPointerOverGameObject check for touches
        for (int i = 0; i < Input.touchCount; ++i)
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                return true;

        // OnGUI check
        return GUIUtility.hotControl != 0;
    }

    ///// <summary>
    ///// 带有[Serializable]和[MessagePackObject]标志的类型
    ///// </summary>
    ///// <param name="t"></param>
    ///// <returns></returns>
    //public static bool IsSerializableType(Type t)
    //{
    //	return t.IsSerializable && Attribute.IsDefined(t, typeof(MessagePackObjectAttribute));
    //}

    /// <summary>
    /// 取出所有无[NonSerialized]字段
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static IEnumerable<FieldInfo> GetSerializableFields(Type t)
    {
        var serializableFields = t
            .GetFields(BindingFlags.Public /*| BindingFlags.NonPublic*/ | BindingFlags.Instance)
            .Where(fi => !Attribute.IsDefined(fi, typeof(NonSerializedAttribute)));
        return serializableFields;
    }

    #region Regex & Pwd
    private const string EMAIL_PATTERN = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
    private const string USERNAME_AND_DISCRIMINATOR_PATTERN = @"^[a-zA-Z0-9]{4,20}#[0-9]{4}$";
    private const string USERNAME_PATTERN = @"^[a-zA-Z0-9]{4,20}$";
    private const string RANDOM_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private static System.Random rnd = new System.Random();

    public static bool IsEmail(string email) { return email != null && Regex.IsMatch(email, EMAIL_PATTERN); }
    public static bool IsUsername(string username) { return username != null && Regex.IsMatch(username, USERNAME_PATTERN); }
    public static bool IsUsernameAndDiscriminator(string usernameAndDiscriminator) { return usernameAndDiscriminator != null && Regex.IsMatch(usernameAndDiscriminator, USERNAME_AND_DISCRIMINATOR_PATTERN); }
    public static string GenerateRandom(int length) { return new string(Enumerable.Repeat(RANDOM_CHARS, length).Select(s => s[rnd.Next(s.Length)]).ToArray()); }

    public static string SHA256(string password)
    {
        byte[] hashValue = null;
        using (var sha = new SHA256Managed())
        {
            hashValue = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        return BitConverter.ToString(hashValue);
    }
    #endregion

    // string.GetHashCode is not quaranteed to be the same on all machines, but
    // we need one that is the same on all machines. simple and stupid:
    public static int GetStableHashCode(this string text)
    {
        unchecked
        {
            int hash = 23;
            foreach (char c in text)
                hash = hash * 31 + c;
            return hash;
        }
    }


    // hard mouse scrolling that is consistent between all platforms
    //   Input.GetAxis("Mouse ScrollWheel") and
    //   Input.GetAxisRaw("Mouse ScrollWheel")
    //   both return values like 0.01 on standalone and 0.5 on WebGL, which
    //   causes too fast zooming on WebGL etc.
    // normally GetAxisRaw should return -1,0,1, but it doesn't for scrolling
    public static float GetAxisRawScrollUniversal()
    {
        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        if (scroll < 0) return -1;
        if (scroll > 0) return 1;
        return 0;
    }

    // two finger pinch detection
    // source: https://docs.unity3d.com/Manual/PlatformDependentCompilation.html
    public static float GetPinch()
    {
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            return touchDeltaMag - prevTouchDeltaMag;
        }
        return 0;
    }

    // universal zoom: mouse scroll if mouse, two finger pinching otherwise
    public static float GetZoomUniversal()
    {
        if (Input.mousePresent)
            return Utils.GetAxisRawScrollUniversal();
        else if (Input.touchSupported)
            return GetPinch();
        return 0;
    }

    public static bool IsSameBlock(this Vector3 v1, Vector3 v2)
    {
        const int BLOCK_SIZE = 4;
        return ((int)v1.x / BLOCK_SIZE) == ((int)v2.x / BLOCK_SIZE)
            && ((int)v1.z / BLOCK_SIZE) == ((int)v2.z / BLOCK_SIZE);
    }

    public static void CopyDirectory(string srcPath, string destPath)
    {
        try
        {
            DirectoryInfo dir = new DirectoryInfo(srcPath);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //获取目录下（不包含子目录）的文件和子目录
            foreach (FileSystemInfo i in fileinfo)
            {
                if (i is DirectoryInfo)     //判断是否文件夹
                {
                    if (!Directory.Exists(destPath + "\\" + i.Name))
                    {
                        Directory.CreateDirectory(destPath + "\\" + i.Name);   //目标目录下不存在此文件夹即创建子文件夹
                    }
                    CopyDirectory(i.FullName, destPath + "\\" + i.Name);    //递归调用复制子文件夹
                }
                else
                {
                    File.Copy(i.FullName, destPath + "\\" + i.Name, true);      //不是文件夹即复制文件，true表示可以覆盖同名文件
                }
            }
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public static string GetMd5(string fileName)
    {
        string result = "";
        using (FileStream fs = File.OpenRead(fileName))
        {
            result = BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(fs));
            fs.Close();
        }
        return result;
    }
}

