using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundles : EditorWindow
{
    private class AssetBundleInfo 
    {
        public bool build = true;
        public string name;
        public AssetBundleBuild buildStruct;
    }

    Vector2 scrollPos;
    bool buildAllBundles = true;
    List<AssetBundleInfo> bundlesInfo = new List<AssetBundleInfo>();

    public CreateAssetBundles()
    {
        var assetsBundlesPaths = Directory.GetDirectories("Assets/AssetsBundles/");
        foreach (var bundlePath in assetsBundlesPaths)
        {
            var bundleName = bundlePath.Split('/').Last();

            var bundleBuildInfo = new AssetBundleBuild();
            bundleBuildInfo.assetBundleName = Path.Combine(bundleName, bundleName);
            bundleBuildInfo.assetNames = new string[] { bundlePath };

            bundlesInfo.Add(new AssetBundleInfo { build = true, name = bundleName, buildStruct = bundleBuildInfo });
        }
    }

    void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        {
            if (GUILayout.Button("Build"))
            {
                var buildMap = new List<AssetBundleBuild>();
                foreach (var bundleInfo in bundlesInfo)
                {
                    if (bundleInfo.build)
                    {
                        buildMap.Add(bundleInfo.buildStruct);
                    }
                }

                var assetsBundlesOutPath = "Assets/AssetsBundlesOut";
                CheckOutBundlesDirectory(assetsBundlesOutPath);
                BuildPipeline.BuildAssetBundles(assetsBundlesOutPath, buildMap.ToArray(), BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
            }

            var buildAllBundlesPrev = buildAllBundles;
            buildAllBundles = EditorGUILayout.ToggleLeft("", buildAllBundles);
            if (buildAllBundlesPrev != buildAllBundles)
            {
                foreach (var bundleInfo in bundlesInfo)
                {
                    bundleInfo.build = buildAllBundles;
                }
            }

            foreach (var bundleInfo in bundlesInfo)
            {
                var bundleInfoBuildPrev = bundleInfo.build;
                bundleInfo.build = EditorGUILayout.ToggleLeft(bundleInfo.name, bundleInfo.build);
                if (bundleInfoBuildPrev != bundleInfo.build)
                {
                    bool all = true;
                    foreach (var bi in bundlesInfo)
                    {
                        all &= bi.build;
                    }
                    buildAllBundles = all;
                }
            }
        }

        EditorGUILayout.EndScrollView();
    }

    [MenuItem("Assets/Build selected AssetBundles (buildMap)")]
    static void BuildAssetBundlesBuildMapWithForm()
    {
        GetWindow<CreateAssetBundles>(true, "CreateAssetBundles", true);
    }

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
}