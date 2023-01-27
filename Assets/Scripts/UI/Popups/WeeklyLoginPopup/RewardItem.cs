using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSWeeklyLoginPopup
    {
        public class RewardItem : MonoBehaviour
        {
            private GameObject active;
            private GameObject claimed;
            private GameObject upcoming;
            private Image itemImage;
            private GameObject markerClaimed;
            private Button claimButton;

            private void Awake()
            {
                active = transform.Find("Active").gameObject;
                claimed = transform.Find("Claimed").gameObject;
                upcoming = transform.Find("Upcoming").gameObject;
                itemImage = transform.Find("ItemImage").GetComponent<Image>();
                markerClaimed = itemImage.transform.Find("MarkerClaimed").gameObject;
                claimButton = transform.Find("ClaimButton").GetComponent<Button>();
                claimButton.onClick.AddListener(ClaimButtonClick);
            }

            private void ClaimButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            }

            public void Customize()
            {
                
            }
        }
    }
}
