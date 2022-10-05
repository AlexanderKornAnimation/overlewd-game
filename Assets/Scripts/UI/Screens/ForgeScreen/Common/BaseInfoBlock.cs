using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSForgeScreen
    {
        public abstract class BaseInfoBlock : MonoBehaviour
        {
            protected TextMeshProUGUI consumeCount;
            protected Image consumeIcon;

            protected TextMeshProUGUI targetCount;
            protected Image targetIcon;
            protected Button decButton;
            protected Button incButton;

            protected virtual void Awake()
            {
                var consumeCell = transform.Find("ConsumeCell");
                consumeCount = consumeCell.Find("Counter/Count").GetComponent<TextMeshProUGUI>();
                consumeIcon = consumeCell.Find("Icon").GetComponent<Image>();

                var targetCell = transform.Find("TargetCell");
                targetCount = targetCell.Find("Counter/Count").GetComponent<TextMeshProUGUI>();
                decButton = targetCell.Find("Counter/Dec").GetComponent<Button>();
                decButton.onClick.AddListener(DecClick);
                incButton = targetCell.Find("Counter/Inc").GetComponent<Button>();
                incButton.onClick.AddListener(IncClick);
                targetIcon = targetCell.Find("Icon").GetComponent<Image>();
            }

            protected virtual void IncClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            }

            protected virtual void DecClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            }

            public int targetCountValue => int.Parse(targetCount.text);
        }
    }
}
