using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Overlewd
{
    

    public class BuildInfoWidget : MonoBehaviour
    {
        [Serializable]
        private class BuildManifest
        {
            public string scmCommitId;
            public string scmBranch;
            public string buildNumber;
            public string buildStartTime;
            public string projectId;
            public string bundleId;
            public string unityVersion;
            public string xcodeVersion;
            public string cloudBuildTargetName;
        }

        private TextMeshProUGUI info;

        void Awake()
        {
            info = transform.GetComponentInChildren<TextMeshProUGUI>();
        }

        void Start()
        {
            var cloudManifest = Resources.Load<TextAsset>("UnityCloudBuildManifest.json");
            var buildManifest = JsonUtility.FromJson<BuildManifest>(cloudManifest?.text);

            info.text =
                $"userId: {GameData.player.info?.id}\n" +
                $"nutakuUserId: {NutakuApiHelper.loginInfo?.userId}\n" +
                $"device: {SystemInfo.deviceModel}\n" +
                $"platform: {Application.platform}\n" +
                $"buildVersion: {Application.version}\n" +
                $"cloudBuildTime: {buildManifest?.buildStartTime}\n" +
                $"cloudBuildVersion: {buildManifest?.cloudBuildTargetName}-{buildManifest?.buildNumber}\n" +
                $"serverURL: {BuildParameters.ServerDomainURL}";
        }

        public static BuildInfoWidget GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateScreenPrefab<BuildInfoWidget>
                ("Prefabs/UI/Widgets/Notifications/System/BuildInfoWidget", parent);
        }
    }
}

