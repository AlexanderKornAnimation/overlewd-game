using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSForgeScreen
    {
        public class InfoBlockBattleGirlEquip : InfoBlockEquipBase
        {
            public BattleGirlsEquipContent equipCtrl { get; set; }

            protected override void IncClick()
            {
                base.IncClick();
                AutoFilled();
                equipCtrl.RefreshState();
            }

            protected override void DecClick()
            {
                base.DecClick();
                AutoFilled();
                equipCtrl.RefreshState();
            }

            public override AdminBRO.ForgePrice.MergeEquipmentSettings mergeSettings =>
                GameData.buildings.forge.prices.mergeEquipmentCharacterSettings;
        }
    }
}
