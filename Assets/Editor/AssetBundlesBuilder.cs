using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AssetBundlesBuilder : EditorWindow
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

    private static void CheckOutBundlesDirectory(string dirPath)
    {
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
    }

    public AssetBundlesBuilder()
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

                var assetsBundlesOutPathWindows = "Assets/AssetsBundlesOut/Windows";
                CheckOutBundlesDirectory(assetsBundlesOutPathWindows);
                BuildPipeline.BuildAssetBundles(assetsBundlesOutPathWindows, buildMap.ToArray(), BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);

                var assetsBundlesOutPathAndroid = "Assets/AssetsBundlesOut/Android";
                CheckOutBundlesDirectory(assetsBundlesOutPathAndroid);
                BuildPipeline.BuildAssetBundles(assetsBundlesOutPathAndroid, buildMap.ToArray(), BuildAssetBundleOptions.None, BuildTarget.Android);
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

    [MenuItem("Assets/Open AssetBundles builder")]
    static void OpenAssetBundlesBuilder()
    {
        GetWindow<AssetBundlesBuilder>(true, "CreateAssetBundles", true);
    }
}