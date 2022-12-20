using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using UnityEditor.Android;
using UnityEngine;

public class ModifyUnityAndroidAppManifestSample : IPostGenerateGradleAndroidProject
{
    private const string AndroidXmlNamespace = "{http://schemas.android.com/apk/res/android}";

    public void OnPostGenerateGradleAndroidProject(string basePath)
    {
        // If needed, add condition checks on whether you need to run the modification routine.
        // For example, specific configuration/app options enabled

        //change Nutaku app params
        RewriteNutakuParams(basePath);

        //second way
        //var androidManifest = new AndroidManifest(GetManifestPath(basePath));
        //androidManifest.SetMicrophonePermission();
        // Add your XML manipulation routines
        //androidManifest.Save();
    }

    public int callbackOrder { get { return 1; } }

    private string _manifestFilePath;

    private string GetManifestPath(string basePath)
    {
        if (string.IsNullOrEmpty(_manifestFilePath))
        {
            var pathBuilder = new StringBuilder(basePath);
            pathBuilder.Append(Path.DirectorySeparatorChar).Append("src");
            pathBuilder.Append(Path.DirectorySeparatorChar).Append("main");
            pathBuilder.Append(Path.DirectorySeparatorChar).Append("AndroidManifest.xml");
            _manifestFilePath = pathBuilder.ToString();
        }
        return _manifestFilePath;
    }

    public void RewriteNutakuParams(string basePath)
    {
        if (Overlewd.GitRepoParams.isMaster)
        {
            XDocument doc = XDocument.Load(GetManifestPath(basePath));
            var eApp = doc.Root.Element("application");
            var eAppMetaData = eApp.Elements("meta-data");
            var e_sdk_ai = eAppMetaData.FirstOrDefault(e => e.Attribute($"{AndroidXmlNamespace}name")?.Value == "sdk_ai");
            var e_sdk_ck = eAppMetaData.FirstOrDefault(e => e.Attribute($"{AndroidXmlNamespace}name")?.Value == "sdk_ck");
            var e_sdk_cs = eAppMetaData.FirstOrDefault(e => e.Attribute($"{AndroidXmlNamespace}name")?.Value == "sdk_cs");
            var e_sdk_en = eAppMetaData.FirstOrDefault(e => e.Attribute($"{AndroidXmlNamespace}name")?.Value == "sdk_en");
            e_sdk_ai?.SetAttributeValue($"{AndroidXmlNamespace}value", "25308");
            e_sdk_ck?.SetAttributeValue($"{AndroidXmlNamespace}value", "hKvfDU43X7sgom34");
            e_sdk_cs?.SetAttributeValue($"{AndroidXmlNamespace}value", "vwpLY2DiKiGVki-tYszmatf5rA=HP5B#");
            e_sdk_en?.SetAttributeValue($"{AndroidXmlNamespace}value", "sandbox");
            e_sdk_en?.Remove();
            doc.Save(_manifestFilePath);
        }
    }
}


internal class AndroidXmlDocument : XmlDocument
{
    private string m_Path;
    protected XmlNamespaceManager nsMgr;
    public readonly string AndroidXmlNamespace = "http://schemas.android.com/apk/res/android";
    public AndroidXmlDocument(string path)
    {
        m_Path = path;
        using (var reader = new XmlTextReader(m_Path))
        {
            reader.Read();
            Load(reader);
        }
        nsMgr = new XmlNamespaceManager(NameTable);
        nsMgr.AddNamespace("android", AndroidXmlNamespace);
    }

    public string Save()
    {
        return SaveAs(m_Path);
    }

    public string SaveAs(string path)
    {
        using (var writer = new XmlTextWriter(path, new UTF8Encoding(false)))
        {
            writer.Formatting = Formatting.Indented;
            Save(writer);
        }
        return path;
    }
}


internal class AndroidManifest : AndroidXmlDocument
{
    private readonly XmlElement ApplicationElement;

    public AndroidManifest(string path) : base(path)
    {
        ApplicationElement = SelectSingleNode("/manifest/application") as XmlElement;
    }

    private XmlAttribute CreateAndroidAttribute(string key, string value)
    {
        XmlAttribute attr = CreateAttribute("android", key, AndroidXmlNamespace);
        attr.Value = value;
        return attr;
    }

    internal XmlNode GetActivityWithLaunchIntent()
    {
        return SelectSingleNode("/manifest/application/activity[intent-filter/action/@android:name='android.intent.action.MAIN' and " +
                "intent-filter/category/@android:name='android.intent.category.LAUNCHER']", nsMgr);
    }

    internal void SetApplicationTheme(string appTheme)
    {
        ApplicationElement.Attributes.Append(CreateAndroidAttribute("theme", appTheme));
    }

    internal void SetStartingActivityName(string activityName)
    {
        GetActivityWithLaunchIntent().Attributes.Append(CreateAndroidAttribute("name", activityName));
    }


    internal void SetHardwareAcceleration()
    {
        GetActivityWithLaunchIntent().Attributes.Append(CreateAndroidAttribute("hardwareAccelerated", "true"));
    }

    internal void SetMicrophonePermission()
    {
        var manifest = SelectSingleNode("/manifest");
        XmlElement child = CreateElement("uses-permission");
        manifest.AppendChild(child);
        XmlAttribute newAttribute = CreateAndroidAttribute("name", "android.permission.RECORD_AUDIO");
        child.Attributes.Append(newAttribute);
    }
}