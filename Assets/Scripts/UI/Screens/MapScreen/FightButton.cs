using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMapScreen
    {
        public class FightButton : MonoBehaviour
        {
            protected Button button;
            protected Transform fightDone;

            protected TextMeshProUGUI title;
            protected TextMeshProUGUI loot;

            protected GameObject icon;
            protected GameObject bossIcon;
            protected GameObject markers;
            protected GameObject eventMark1;
            protected GameObject eventMark2;
            protected GameObject eventMark3;
            protected GameObject mainQuestMark;
            protected GameObject sideQuestMark;

            private void Awake()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                
                title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                loot = button.transform.Find("Loot").GetComponent<TextMeshProUGUI>();
                
                fightDone = button.transform.Find("FightDone");

                icon = button.transform.Find("Icon").gameObject;
                bossIcon = button.transform.Find("BossIcon").gameObject;
                markers = button.transform.Find("Markers").gameObject;
                eventMark1 = markers.transform.Find("EventMark1").gameObject;
                eventMark2 = markers.transform.Find("EventMark2").gameObject;
                eventMark3 = markers.transform.Find("EventMark3").gameObject;
                mainQuestMark = markers.transform.Find("MainQuestMark").gameObject;
                sideQuestMark = markers.transform.Find("SideQuestMark").gameObject;

                button.onClick.AddListener(ButtonClick);
            }

            protected virtual void ButtonClick()
            {
                SoundManager.PlayOneShoot(SoundPath.UI_GenericButtonClick);
                UIManager.ShowScreen<PrepareBattlePopup>();
            }
            
            public static FightButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<FightButton>
                    ("Prefabs/UI/Screens/MapScreen/FightButton", parent);
            }
        }
    }
}

