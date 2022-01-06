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
}