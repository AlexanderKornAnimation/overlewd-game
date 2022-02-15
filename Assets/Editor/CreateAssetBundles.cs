using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundles
{
	private static void CheckOutBundlesDirectory(string dirPath)
    {
		if (!Directory.Exists(dirPath))
		{
			Directory.CreateDirectory(dirPath);
		}
	}

    /*[MenuItem("Assets/Build AssetBundles")]
	static void BuildAssetBundles()
	{
		var assetsBundlesOutPath = "Assets/AssetsBundlesOut";
        CheckOutBundlesDirectory(assetsBundlesOutPath);
		BuildPipeline.BuildAssetBundles(assetsBundlesOutPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
	}*/

    [MenuItem("Assets/Build AssetBundles (buildMap)")]
    static void BuildAssetBundlesBuildMap()
    {
        var buildMap = new List<AssetBundleBuild>();
        var assetsBundlesPaths = Directory.GetDirectories("Assets/AssetsBundles/");
        foreach (var bundlePath in assetsBundlesPaths) 
        {
            var bundleName = bundlePath.Split('/').Last();

            var bundleBuildInfo = new AssetBundleBuild();
            bundleBuildInfo.assetBundleName = Path.Combine(bundleName, bundleName);
            bundleBuildInfo.assetNames = new string[] { bundlePath };
            buildMap.Add(bundleBuildInfo);
        }

        var assetsBundlesOutPath = "Assets/AssetsBundlesOut";
        CheckOutBundlesDirectory(assetsBundlesOutPath);
        BuildPipeline.BuildAssetBundles(assetsBundlesOutPath, buildMap.ToArray(), BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}