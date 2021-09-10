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

	[MenuItem("Assets/Build AssetBundles For Windows")]
	static void BuildAssetBundlesForWindows()
	{
		var bundlesPath = "Assets/WindowsAssetsBundles";
		CheckBundlesDirectory(bundlesPath);
		BuildPipeline.BuildAssetBundles(bundlesPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
	}

	[MenuItem("Assets/Build AssetBundles For Android")]
	static void BuildAssetBundlesForAndroid()
	{
		var bundlesPath = "Assets/AndoidAssetsBundles";
		CheckBundlesDirectory(bundlesPath);
		BuildPipeline.BuildAssetBundles(bundlesPath, BuildAssetBundleOptions.None, BuildTarget.Android);
	}
}