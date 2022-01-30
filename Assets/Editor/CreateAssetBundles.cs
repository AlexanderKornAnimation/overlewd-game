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

        buildMap[0].assetBundleName = "enemybundle";
        string[] enemyAssets = new string[2];
        enemyAssets[0] = "Assets/Textures/char_enemy_alienShip.jpg";
        enemyAssets[1] = "Assets/Textures/char_enemy_alienShip-damaged.jpg";
        buildMap[0].assetNames = enemyAssets;

        var bundlesPath = "Assets/AssetsBundles";
        CheckBundlesDirectory(bundlesPath);
        BuildPipeline.BuildAssetBundles(bundlesPath, buildMap, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}