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

            protected virtual void Awake()
            {
                canvas = transform.Find("Canvas");
                title = canvas.Find("Title").GetComponent<TextMeshProUGUI>();
                timer = canvas.Find("Timer").gameObject;
                timerTitle = timer.transform.Find("Title").GetComponent<TextMeshProUGUI>();

                UIManager.widgetsGameDataListeners += OnGameDataEvent;
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

            protected void MakeSummonOneButton(Button button, TextMeshProUGUI title)
            {
                var _gachaData = gachaData;

                button.gameObject.SetActive(_gachaData?.priceForOne?.Count > 0);
                if (button.gameObject.activeSelf)
                {
                    var entityName = _gachaData.tabType switch
                    {
                        AdminBRO.GachaItem.TabType_Characters => "battle girl",
                        AdminBRO.GachaItem.TabType_CharactersEquipment => "equipment",
                        AdminBRO.GachaItem.TabType_OverlordEquipment => "equipment",
                        AdminBRO.GachaItem.TabType_MatriachsShards => "memory",
                        _ => "-"
                    };
                    title.text = $"Summon 1 {entityName} for " + UITools.PriceToString(_gachaData.priceForOne);
                    var canSummon = GameData.player.CanBuy(_gachaData.priceForOne) && _gachaData.available;
                    UITools.DisableButton(button, !canSummon);
                }
            }

            protected void MakeSummonManyButton(Button button, TextMeshProUGUI title)
            {
                var _gachaData = gachaData;

                button.gameObject.SetActive(_gachaData?.priceForMany?.Count > 0);
                if (button.gameObject.activeSelf)
                {
                    var entityName = _gachaData.tabType switch
                    {
                        AdminBRO.GachaItem.TabType_Characters => "battle girls",
                        AdminBRO.GachaItem.TabType_CharactersEquipment => "equipments",
                        AdminBRO.GachaItem.TabType_OverlordEquipment => "equipments",
                        AdminBRO.GachaItem.TabType_MatriachsShards => "memories",
                        _ => "-"
                    };
                    title.text = $"Summon 5 {entityName} for " + UITools.PriceToString(_gachaData.priceForMany);
                    var canSummon = GameData.player.CanBuy(_gachaData.priceForMany) && _gachaData.available;
                    UITools.DisableButton(button, !canSummon);
                }
            }

            private IEnumerator TimerUpd()
            {
                timerTitle.text = gachaData?.timePeriodLeft;
                yield return new WaitForSeconds(1.0f);
            }
        }
    }
}