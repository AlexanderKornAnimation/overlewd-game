using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

/*
autoinc build version
https://github.com/PimDeWitte/UnityAutoIncrementBuildVersion
*/


public class OverlewdBuildProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
    public const string MajorBuildVersion = "1";
    public const string MinorBuildVersion = Overlewd.AdminBRO.ApiVersion;

    public int callbackOrder { get { return 0; } }

    public void OnPostprocessBuild(BuildReport report)
    {
        //PlayerSettings.bundleVersion = "1.0.3";
        //PlayerSettings.Android.bundleVersionCode = 2;
        //AssetDatabase.SaveAssets();
    }

    public void OnPreprocessBuild(BuildReport report)
    {
        
    }

    [MenuItem("SupaDogeTools/Inc Build Version")]
    static void IncBuildVersion()
    {
        PlayerSettings.Android.bundleVersionCode++;
        PlayerSettings.bundleVersion = $"{MajorBuildVersion}.{MinorBuildVersion}.{PlayerSettings.Android.bundleVersionCode}";
        AssetDatabase.SaveAssets();
    }
}
