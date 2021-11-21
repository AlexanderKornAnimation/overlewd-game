using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSQuestOverlay
    {
        public class MainQuestInfo : MonoBehaviour
        {
            protected Text title;
            protected Text progress;

            protected virtual void Awake()
            {
                var canvas = transform.Find("Canvas");

                title = canvas.Find("QuestHead").Find("Title").GetComponent<Text>();
                progress = canvas.Find("QuestHead").Find("Progress").GetComponent<Text>();
            }

            protected static GameObject LoadPrefab(Transform parent)
            {
                return (GameObject)Instantiate(Resources.Load("Prefabs/UI/Overlays/QuestOverlay/MainQuestInfo"), parent);
            }

            public static MainQuestInfo GetInstance(Transform parent)
            {
                var newItem = LoadPrefab(parent);
                newItem.name = nameof(MainQuestInfo);
                return newItem.AddComponent<MainQuestInfo>();
            }
        }
    }
}