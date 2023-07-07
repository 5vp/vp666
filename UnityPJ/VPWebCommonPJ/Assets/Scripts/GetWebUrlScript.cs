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

    /** 获取URL上的参数 */
    public static string GetWebUrlParam(string key)
    {
        if(paramsMap.ContainsKey(key)){
            return paramsMap[key];
        }
        if(key.Equals("scene")){
            return "Scenes/CommonSTScene";// 默认场景
        }else if(key.Equals("pos_x")){
            return "0";
        }else if(key.Equals("pos_y")){
            return "0";
        }else if(key.Equals("pos_z")){
            return "0";
        }else if(key.Equals("rot_x")){
            return "0";
        }else if(key.Equals("rot_y")){
            return "0";
        }else if(key.Equals("rot_z")){
            return "0";
        }else if(key.Equals("scl_x")){
            return "1";
        }else if(key.Equals("scl_y")){
            return "1";
        }else if(key.Equals("scl_z")){
            return "1";
        }
        return "";
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InitUrlData()
    {
        //paramsMap["shaderType"] = "shadow";
        
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