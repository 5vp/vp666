using UnityEditor;
using UnityEngine;

public class AssetBundleBuilder : Editor
{
    [MenuItem("AssetBundles/Build AssetBundles")]
    static void BuildAssetBundles()
    {
        // 设置打包输出路径和资源列表
        AssetBundleBuild[] builds = new AssetBundleBuild[1];
        builds[0].assetBundleName = "dajibundle";
        builds[0].assetNames = new string[] { "Assets/DaJi/DaJiPrefeb.prefab"};

        // 执行打包操作
        BuildPipeline.BuildAssetBundles("DLC", builds, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}