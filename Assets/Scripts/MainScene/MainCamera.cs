using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MainCamera : MonoBehaviour
{
    [SerializeField]
    private NetworkHelper networkHelper;

    [SerializeField]
    private ResourceManager resourceManager;

    [SerializeField]
    private Player player;

    void Awake()
    {
        resourceManager.InitializeCache();

        StartCoroutine(networkHelper.Authorization(e =>
        {
            StartCoroutine(player.GetPlayerInfo(e =>
            {
                StartCoroutine(player.ChangeName("NewName", e =>
                {
                    
                }));
            }));

            StartCoroutine(resourceManager.LoadResourcesMeta(meta =>
            {
                var metaFilePath = resourceManager.GetCacheFilePath("ResourcesMeta");
                if (resourceManager.FileExists(metaFilePath))
                {
                    resourceManager.FileDelete(metaFilePath);
                }
                resourceManager.CacheText(metaFilePath, JsonUtility.ToJson(meta));

                var existingFiles = resourceManager.GetFileNamesFormResources();

                StartCoroutine(resourceManager.DeleteNotRelevantResources(meta, existingFiles));
                StartCoroutine(resourceManager.DownloadMissingResources(meta, existingFiles));
            }));
        }));
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
