using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSGuestScreen
    {
        public class Guest : MonoBehaviour
        {
            private GameObject unlocked;
            private GameObject active;
            private GameObject locked;
            private Button button;
            private Image girlArt;
            private TextMeshProUGUI girlName;
            
            public int? girlId { get; set; }
            public AdminBRO.MatriarchItem girlData => GameData.matriarchs.GetMatriarchById(girlId);
            
            private void Awake()
            {
                unlocked = transform.Find("Unlocked").gameObject;
                active = transform.Find("Active").gameObject;
                locked = transform.Find("Locked").gameObject;
                button = transform.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
                girlArt = transform.Find("Mask/GirlArt").GetComponent<Image>();
                girlName = transform.Find("GirlName").GetComponent<TextMeshProUGUI>();
            }

            private void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.MakeScreen<GuestMemoryListScreen>().
                    SetData(new GuestMemoryListScreenInData
                    {
                        girlId = girlId,
                    }).DoShow();
            }

            public void Customize()
            {
                var today = DateTime.Today;
                var index = transform.GetSiblingIndex();

                if (today.Month - 1 == index)
                {
                    locked.SetActive(false);
                    active.SetActive(true);
                }
            }
        }
    }
}
