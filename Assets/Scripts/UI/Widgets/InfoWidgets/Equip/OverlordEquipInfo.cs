using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Overlewd
{
    public class OverlordEquipInfo : BaseEquipInfo
    {
        private GameObject speedBack;
        private TextMeshProUGUI speedStat;

        private GameObject powerBack;
        private TextMeshProUGUI powerStat;

        private GameObject constitutionBack;
        private TextMeshProUGUI constitutionStat;

        private GameObject agilityBack;
        private TextMeshProUGUI agilityStat;

        protected override void Awake()
        {
            base.Awake();
            speedBack = background.Find("Speed").gameObject;
            speedStat = speedBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
            
            powerBack = background.Find("Power").gameObject;
            powerStat = powerBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
            
            constitutionBack = background.Find("Constitution").gameObject;
            constitutionStat = constitutionBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
            
            agilityBack = background.Find("Agility").gameObject;
            agilityStat = agilityBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
        }

        protected override void Customize()
        {
            base.Customize();

            if (eqData != null)
            {
                speedStat.text = "+" + eqData.speed;
                speedBack.SetActive(eqData.speed != 0);

                powerStat.text = "+" + eqData.power;
                powerBack.SetActive(eqData.power != 0);

                constitutionStat.text = "+" + eqData.constitution;
                constitutionBack.SetActive(eqData.constitution != 0.0f);

                agilityStat.text = "+" + eqData.agility;
                agilityBack.SetActive(eqData.agility != 0.0f);
            }
        }

        public static OverlordEquipInfo GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateWidgetPrefab<OverlordEquipInfo>(
                "Prefabs/UI/Widgets/InfoWidgets/OverlordEquipInfo", parent);
        }
    }
}