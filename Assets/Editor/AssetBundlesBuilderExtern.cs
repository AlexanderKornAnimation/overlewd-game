using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AssetBundlesBuilderExtern : EditorWindow
{
    private class AssetBundleInfo
    {
        public string name;
        public string assetPath;
    }

    private Vector2 scrollPos;
    private bool singleBundle = true;
    private string singleBundleName = "bundle_name";

    private List<AssetBundleInfo> bundlesInfo = new List<AssetBundleInfo>();

    private static void CheckOutBundlesDirectory(string dirPath)
    {
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
    }

    public AssetBundlesBuilderExtern()
    {
        
    }

    void OnEnable()
    {
        var objs = Selection.GetFiltered<Object>(SelectionMode.Assets);
        foreach (var obj in objs)
        {
            string assetPath = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(assetPath))
            {
                if (Directory.Exists(assetPath))
                {
                    var dirName = assetPath.Split('/').Last();
                    bundlesInfo.Add(new AssetBundleInfo { assetPath = assetPath, name = dirName });
                }

                if (File.Exists(assetPath))
                {
                    var fileName = assetPath.Split('/').Last().Split('.').First();
                    bundlesInfo.Add(new AssetBundleInfo { assetPath = assetPath, name = fileName });
                }
            }
        }

        if (bundlesInfo.Count == 1)
        {
            singleBundleName = bundlesInfo.First().name;
        }
    }

    void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        {
            if (bundlesInfo.Count > 1)
            {
                singleBundle = EditorGUILayout.ToggleLeft("Single bundle", singleBundle);
            }

            if (singleBundle)
            {
                EditorGUILayout.LabelField("Bundle Name:");
                singleBundleName = EditorGUILayout.TextField(singleBundleName);
            }

            if (GUILayout.Button("Build"))
            {
                var buildMap = new List<AssetBundleBuild>();

                if (singleBundle)
                {
                    var bundleBuildInfo = new AssetBundleBuild();
                    bundleBuildInfo.assetBundleName = Path.Combine(singleBundleName, singleBundleName);
                    var assetNames = new List<string>();
                    foreach (var item in bundlesInfo)
                    {
                        assetNames.Add(item.assetPath);
                    }
                    bundleBuildInfo.assetNames = assetNames.ToArray();
                    buildMap.Add(bundleBuildInfo);
                }
                else
                {
                    foreach (var item in bundlesInfo)
                    {
                        var bundleBuildInfo = new AssetBundleBuild();
                        bundleBuildInfo.assetBundleName = Path.Combine(item.name, item.name);
                        bundleBuildInfo.assetNames = new string[] { item.assetPath };
                        buildMap.Add(bundleBuildInfo);
                    }
                }

                var assetsBundlesOutPathWindows = "Assets/AssetsBundlesOut/Windows";
                CheckOutBundlesDirectory(assetsBundlesOutPathWindows);
                BuildPipeline.BuildAssetBundles(assetsBundlesOutPathWindows, buildMap.ToArray(), BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);

                var assetsBundlesOutPathAndroid = "Assets/AssetsBundlesOut/Android";
                CheckOutBundlesDirectory(assetsBundlesOutPathAndroid);
                BuildPipeline.BuildAssetBundles(assetsBundlesOutPathAndroid, buildMap.ToArray(), BuildAssetBundleOptions.None, BuildTarget.Android);

                var assetsBundlesOutPathWebGL = "Assets/AssetsBundlesOut/WebGL";
                CheckOutBundlesDirectory(assetsBundlesOutPathWebGL);
                BuildPipeline.BuildAssetBundles(assetsBundlesOutPathWebGL, buildMap.ToArray(), BuildAssetBundleOptions.None, BuildTarget.WebGL);
            }

            EditorGUILayout.LabelField("");
            EditorGUILayout.LabelField("Assets:");
            foreach (var item in bundlesInfo)
            {
                EditorGUILayout.LabelField(item.assetPath);
            }
        }
        EditorGUILayout.EndScrollView();
    }

    [MenuItem("Assets/Build AssetBundles")]
    static void OpenAssetBundlesBuilder()
    {
        GetWindow<AssetBundlesBuilderExtern>(true, "CreateAssetBundles", true);
    }
}