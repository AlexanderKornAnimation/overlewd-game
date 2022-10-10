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
        public class InfoBlockOverlordEquip : BaseInfoBlock
        {
            public OverlordEquipContent equipCtrl { get; set; }
            public EquipmentOverlord consumeEquip { get; set; }

            protected override void IncClick()
            {
                base.IncClick();
                equipCtrl.RefreshState();
            }

            protected override void DecClick()
            {
                base.DecClick();
                equipCtrl.RefreshState();
            }

            public void RefreshState()
            {
                if (!isFilled)
                {
                    msgTitle.text = "Tap to equipment wich you want to merge";
                    msgTitle.gameObject.SetActive(true);
                    arrow.gameObject.SetActive(false);
                    consumeCell.gameObject.SetActive(false);
                    targetCell.gameObject.SetActive(false);
                }
                else
                {
                    msgTitle.gameObject.SetActive(false);
                    arrow.gameObject.SetActive(true);
                    consumeCell.gameObject.SetActive(true);
                    targetCell.gameObject.SetActive(true);
                    consumeShardIcon.gameObject.SetActive(false);
                    targetShardIcon.gameObject.SetActive(false);


                }
            }

            public bool isFilled => consumeEquip != null;
        }
    }
}
