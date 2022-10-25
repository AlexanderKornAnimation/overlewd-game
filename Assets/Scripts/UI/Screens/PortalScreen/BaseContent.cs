using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSPortalScreen
    {
        public abstract class BaseContent : BaseWidget
        {
            protected Image contentBackground;
            protected TextMeshProUGUI title;
            protected GameObject discountBack;
            protected TextMeshProUGUI discount;
            protected GameObject timer;
            protected TextMeshProUGUI timerTitle;
            
            protected Transform canvas;
            protected RectTransform rect;
            
            public int? gachaId { get; set; }

            public AdminBRO.GachaItem gachaData => GameData.gacha.GetGachaById(gachaId);

            protected override void Awake()
            {
                base.Awake();

                canvas = transform.Find("Canvas");
                title = canvas.Find("Title").GetComponent<TextMeshProUGUI>();
                timer = canvas.Find("Timer").gameObject;
                timerTitle = timer.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            }

            protected virtual void Start()
            {
                Customize();

                if (gachaData?.isTempOffer ?? false)
                {
                    StartCoroutine(TimerUpd());
                }
                else
                {
                    timer.SetActive(false);
                }
            }

            public virtual void Customize()
            {

            }

            private IEnumerator TimerUpd()
            {
                timerTitle.text = UITools.ChangeTextSize(gachaData?.timePeriodLeft, timerTitle.fontSize);
                yield return new WaitForSeconds(1.0f);
            }
        }
    }
}