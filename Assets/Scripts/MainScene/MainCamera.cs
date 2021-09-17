using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MainCamera : MonoBehaviour
{
    public LoadingScreen loadingScreen;

    void Awake()
    {
        ShowLoadingScreen();
        ResourceManager.InitializeCache();
    }

    void Start()
    {
        if (NetworkHelper.HasNetworkConection())
        {
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

                    StartCoroutine(ResourceManager.ActualizeResources(
                        meta,
                        (resourceMeta) =>
                        {
                            ShowLoadingScreen();
                            loadingScreen.loadingLabel = "Download: " + resourceMeta.url;
                        },
                        () =>
                        {
                            DestroyLoadingScreen();
                            DoLoadResources();
                        }
                    ));
                }));
            }));
        }
        else
        {
            ShowNoInternetMessageBox();
        }
        
    }

    void Update()
    {

    }

    void DoLoadResources()
    {

    }

    void ShowNoInternetMessageBox()
    {
        var box = gameObject.AddComponent<DialogBoxYesNo>();
        box.title = "No Internet Connection";
        box.yesAction = () =>
        {
            Application.Quit();
        };
    }

    void ShowLoadingScreen()
    {
        if (!loadingScreen)
        {
            loadingScreen = gameObject.AddComponent<LoadingScreen>();
        }
    }

    void DestroyLoadingScreen()
    {
        if (loadingScreen)
        {
            DestroyImmediate(loadingScreen);
        }
    }
}
