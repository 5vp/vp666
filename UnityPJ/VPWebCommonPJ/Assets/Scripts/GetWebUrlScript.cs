using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GetWebUrlScript
{
    [DllImport("__Internal")]
    private static extern string StringReturnValueFunction();

    //UrlMsg的数据是 ?a=100&b=vp666
    public static string UrlMsg = string.Empty;

    // 参数字典
    public static Dictionary<string, string> paramsMap = new Dictionary<string, string>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InitUrlData()
    {
#if !UNITY_EDITOR&&UNITY_WEBGL
        UrlMsg = StringReturnValueFunction();
        if(!String.IsNullOrEmpty(UrlMsg))
        {
            UrlMsg = UrlMsg.Substring(1);//去掉问号
            string[] pArr = UrlMsg.Split('&');
            for (int i = 0; i < pArr.Length; i++)
            {
                string[] tArr = pArr[i].Split('=');
                string key = tArr[0];
                string value = tArr[1];
                paramsMap[key] = value;
            }
        }
#endif
    }
}