using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Resharper disable All

namespace Overlewd
{
    namespace FTUE
    {
        namespace NSMapScreen
        {
            public class SexSceneButton : Overlewd.NSMapScreen.SexSceneButton
            {
                public string sexKey { get; set; }

                private void Customize()
                {
                    gameObject.SetActive(true);

                    title.text = "name";
                    sceneDone.gameObject.SetActive(true);
                    button.interactable = true;
                }

                private void Start()
                {
                    Customize();
                }

                protected override void ButtonClick()
                {
                    SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                    UIManager.ShowScreen<CastleScreen>();
                }

                public new static SexSceneButton GetInstance(Transform parent)
                {
                    return ResourceManager.InstantiateWidgetPrefab<SexSceneButton>
                        ("Prefabs/UI/Screens/MapScreen/SexSceneButton", parent);
                }
            }
        }
    }
}