using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class MainCamera : MonoBehaviour
{
    private LoadingScreen loadingScreen;
    private ContentViewer contentViewer;

    void Awake()
    {
        
    }

    void Start()
    {
        ResourceManager.InitializeCache();
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

                StartCoroutine(ResourceManager.GetServerResourcesMeta(serverResourcesMeta =>
                {
                    if (!ResourceManager.HasFreeSpaceForNewResources(serverResourcesMeta))
                    {
                        ShowNoFreeSpaceMessageBox();
                    }
                    else
                    {
                        ResourceManager.SaveLocalResourcesMeta(serverResourcesMeta);
                        ResourceManager.InitRuntimeResourcesMeta(serverResourcesMeta);

                        StartCoroutine(ResourceManager.ActualizeResources(
                            serverResourcesMeta,
                            (resourceItemMeta) =>
                            {
                                ShowLoadingScreen();
                                loadingScreen.loadingLabel = "Download: " + resourceItemMeta.url;
                            },
                            () =>
                            {
                                DestroyLoadingScreen();
                                DoLoadResources();
                            }
                        ));
                    }
                }));
            }));
        }
        else
        {
            var localResourcesMeta = ResourceManager.GetLocalResourcesMeta();
            if (localResourcesMeta != null)
            {
                ResourceManager.InitRuntimeResourcesMeta(localResourcesMeta);
                DoLoadResources();
            }
            else
            {
                ShowNoInternetMessageBox();
            }
        }
        
    }

    void Update()
    {

    }

    void OnGUI()
    {
        
    }

    void DoLoadResources()
    {
        ShowContentViewer();
    }

    void ShowNoInternetMessageBox()
    {
        var dlgBox = gameObject.AddComponent<DialogBoxYesNo>();
        dlgBox.title = "No Internet ñonnection";
        dlgBox.yesAction = () =>
        {
            DestroyImmediate(dlgBox);
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        };
    }

    void ShowNoFreeSpaceMessageBox()
    {
        var dlgBox = gameObject.AddComponent<DialogBoxYesNo>();
        dlgBox.title = "Not enough free space";
        dlgBox.yesAction = () =>
        {
            DestroyImmediate(dlgBox);
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        };
    }

    void ShowLoadingScreen()
    {
        if (loadingScreen == null)
        {
            loadingScreen = gameObject.AddComponent<LoadingScreen>();
        }
    }

    void DestroyLoadingScreen()
    {
        DestroyImmediate(loadingScreen);
    }

    void ShowContentViewer()
    {
        if (contentViewer == null)
        {
            contentViewer = gameObject.AddComponent<ContentViewer>();
        }
    }
}
