using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class CommonSTSceneScript : MonoBehaviour
{
    public string assetBundleURL = "https://vp666.cn/others/unity2022h5/DLC/dajibundle";
    public string assetName = "Assets/DaJi/DaJiPrefeb.prefab";

    void Start()
    {
        StartCoroutine(LoadAssetBundle());
    }

    IEnumerator LoadAssetBundle()
    {
        string aBundleUrl = "https://vp666.cn/others/unity2022h5/DLC/";
        string aburl = GetWebUrlScript.GetWebUrlParam("aburl");
        if(!string.IsNullOrEmpty(aburl)){
            aBundleUrl = aBundleUrl + aburl;
        }else{
            aBundleUrl = assetBundleURL;
        }

        string asset_name = GetWebUrlScript.GetWebUrlParam("asset_name");
        if(!string.IsNullOrEmpty(asset_name)){
            assetName = asset_name;
        }

        // 下载AssetBundle文件
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(aBundleUrl);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            // 加载AssetBundle
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
            if (bundle != null)
            {
                // 加载并实例化资源
                GameObject asset = bundle.LoadAsset<GameObject>(assetName);
                GameObject ins = Instantiate(asset);
                UpdateTransform(ins);
            }
            else
            {
                Debug.Log("Failed to load AssetBundle");
            }

            // 释放AssetBundle
            bundle.Unload(false);
        }
        else
        {
            Debug.Log(www.error);
        }
    }

    void UpdateTransform(GameObject go)
    {
        string pos_x = GetWebUrlScript.GetWebUrlParam("pos_x");
        string pos_y = GetWebUrlScript.GetWebUrlParam("pos_y");
        string pos_z = GetWebUrlScript.GetWebUrlParam("pos_z");
        Vector3 pos = new Vector3(float.Parse(pos_x),float.Parse(pos_y),float.Parse(pos_z));

        string rot_x = GetWebUrlScript.GetWebUrlParam("rot_x");
        string rot_y = GetWebUrlScript.GetWebUrlParam("rot_y");
        string rot_z = GetWebUrlScript.GetWebUrlParam("rot_z");
        Quaternion rot = Quaternion.Euler(float.Parse(rot_x),float.Parse(rot_y),float.Parse(rot_z));

        string scl_x = GetWebUrlScript.GetWebUrlParam("scl_x");
        string scl_y = GetWebUrlScript.GetWebUrlParam("scl_y");
        string scl_z = GetWebUrlScript.GetWebUrlParam("scl_z");
        Vector3 scl = new Vector3(float.Parse(scl_x),float.Parse(scl_y),float.Parse(scl_z));

        go.transform.SetPositionAndRotation(pos,rot);
        go.transform.localScale = scl;
    }
}
