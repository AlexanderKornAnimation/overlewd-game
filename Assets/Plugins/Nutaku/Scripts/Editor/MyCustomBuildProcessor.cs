using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.Android;
using UnityEngine;

class MyCustomBuildProcessor : IPostGenerateGradleAndroidProject
{
    public int callbackOrder { get { return 0; } }
    private const bool showDebugLog = true; // set this to false if you want to not see the output in build log

    public void OnPostGenerateGradleAndroidProject(string path)
    {
        if (showDebugLog)
        {
            Debug.Log("Executing Nutaku SDK's OnPostGenerateGradleAndroidProject(). You may disable this logging in MyCustomBuildProcessor.cs -> showDebugLog");
            Debug.Log("OnPostGenerateGradleAndroidProject > path: " + path);
        }

        string parentPath = path;
        if (Path.GetFileName(path) == "unityLibrary")
        {
            parentPath = Path.GetFullPath(Path.Combine(path, @"..\"));
            parentPath = parentPath.TrimEnd('/');
        }
        if (showDebugLog)
            Debug.Log("OnPostGenerateGradleAndroidProject > parentPath: " + parentPath);

        // enable AndroidX
        string gradlePropertiesFile = parentPath + "/gradle.properties";
        var gradleContentsSb = new StringBuilder();
        if (File.Exists(gradlePropertiesFile))
        {
            if (showDebugLog)
                Debug.Log("OnPostGenerateGradleAndroidProject > gradle.properties exists");
            string[] oldGradlePropertiesLines = File.ReadAllLines(gradlePropertiesFile);
            foreach (string line in oldGradlePropertiesLines)
                if (!line.Contains("android.useAndroidX=") && !line.Contains("android.enableJetifier="))
                    gradleContentsSb.AppendLine(line);
            File.Delete(gradlePropertiesFile);
        }
        StreamWriter writer = File.CreateText(gradlePropertiesFile);
        gradleContentsSb.AppendLine("");
        gradleContentsSb.AppendLine("android.useAndroidX=true");
        gradleContentsSb.AppendLine("android.enableJetifier=true");
        gradleContentsSb.Replace(@"android.enableR8", @"#android.enableR8");

        writer.Write(gradleContentsSb.ToString());
        writer.Flush();
        writer.Close();
        if (showDebugLog)
            Debug.Log("OnPostGenerateGradleAndroidProject > AndroidX enabled");

        // delete existing build.gradle file
        string buildGradle = Path.Combine(path, "build.gradle");
        if (File.Exists(buildGradle))
        {
            File.Delete(buildGradle);
            //Debug.Log("OnPostGenerateGradleAndroidProject > Existing build.gradle deleted");
        }

        // build.gradle copied from mainTemplate.gradle
        FileUtil.CopyFileOrDirectory(Application.dataPath + "/Plugins/Android/mainTemplate.gradle", buildGradle);
        if (showDebugLog)
            Debug.Log("OnPostGenerateGradleAndroidProject > build.grade copied from mainTemplate.gradle");

        // remove **USER_PROGUARD**' comment in build.gradle file
        if (File.Exists(buildGradle))
        {
            string text = File.ReadAllText(buildGradle);
            text = text.Replace("**USER_PROGUARD**", "");
            File.WriteAllText(buildGradle, text);
            if (showDebugLog)
                Debug.Log("OnPostGenerateGradleAndroidProject > **USER_PROGUARD** removed");
        }

        if (showDebugLog)
            Debug.Log("OnPostGenerateGradleAndroidProject() Finished");
    }
}