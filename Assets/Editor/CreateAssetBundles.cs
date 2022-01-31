using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundles
{
	private static void CheckBundlesDirectory(string bundlesPath)
    {
		if (!Directory.Exists(bundlesPath))
		{
			Directory.CreateDirectory(bundlesPath);
		}
	}

	[MenuItem("Assets/Build AssetBundles")]
	static void BuildAssetBundles()
	{
		var bundlesPath = "Assets/AssetsBundles";
		CheckBundlesDirectory(bundlesPath);
		BuildPipeline.BuildAssetBundles(bundlesPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
	}

    [MenuItem("Assets/Build Asset Bundles Using BuildMap")]
    static void BuildAssetBundlesCustom()
    {
        AssetBundleBuild[] buildMap = new AssetBundleBuild[2];

        buildMap[0].assetBundleName = "test_bundle";
        string[] bundleAssetsPath = new string[1];
        bundleAssetsPath[0] = "Assets/AssetsBundles/TestBundle";
        buildMap[0].assetNames = bundleAssetsPath;

        var bundlesPath = "Assets/AssetsBundlesOut";
        CheckBundlesDirectory(bundlesPath);
        BuildPipeline.BuildAssetBundles(bundlesPath, buildMap, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}