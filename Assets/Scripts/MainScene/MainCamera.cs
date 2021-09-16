using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MainCamera : MonoBehaviour
{
    void Awake()
    {
        
    }

    void Start()
    {
        ResourceManager.InitializeCache();

        StartCoroutine(NetworkHelper.Authorization(e =>
        {
            StartCoroutine(Player.GetPlayerInfo(e =>
            {
                StartCoroutine(Player.ChangeName("NewName", e =>
                {

                }));
            }));

            StartCoroutine(ResourceManager.LoadResourcesMeta(meta =>
            {
                var metaFilePath = ResourceManager.GetCacheFilePath("ResourcesMeta");
                if (ResourceManager.FileExists(metaFilePath))
                {
                    ResourceManager.FileDelete(metaFilePath);
                }
                ResourceManager.CacheText(metaFilePath, JsonUtility.ToJson(meta));

                var existingFiles = ResourceManager.GetFileNamesFormResources();

                StartCoroutine(ResourceManager.DeleteNotRelevantResources(meta, existingFiles));
                StartCoroutine(ResourceManager.DownloadMissingResources(meta, existingFiles));
            }));
        }));
    }

    void Update()
    {
        
    }
}
