using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class SummoningScreen : BaseFullScreenParent<SummoningScreenInData>
    {
        private Button haremButton;
        private TextMeshProUGUI haremButtonText;
        private Button overlordButton;
        private Button portalButton;
        private TextMeshProUGUI portalButtonText;
        private Transform canvas;
        private Transform shardsPos;

        private void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/SummoningScreen/SummoningScreen", transform);

            canvas = screenInst.transform.Find("Canvas");
            overlordButton = canvas.Find("OverlordButton").GetComponent<Button>();
            overlordButton.onClick.AddListener(OverlordButtonClick);

            haremButton = canvas.Find("HaremButton").GetComponent<Button>();
            haremButtonText = haremButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            haremButton.onClick.AddListener(HaremButtonClick);

            portalButton = canvas.Find("PortalButton").GetComponent<Button>();
            portalButtonText = portalButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            portalButton.onClick.AddListener(PortalButtonClick);

            shardsPos = canvas.Find("ShardsPos");
        }

        public override async Task AfterShowAsync()
        {
            if (inputData.isFive)
            {
                SoundManager.PlayOneShot(FMODEventPath.Gacha_x10_open);

                NSSummoningScreen.GroupShardsAnimCtrl.GetInstance(shardsPos);
            }
            else
            {
                SoundManager.PlayOneShot(FMODEventPath.Gacha_x1_open);

                NSSummoningScreen.SingleShardAnimCtrl.GetInstance(shardsPos);
            }

            await Task.CompletedTask;
        }

        public override async Task BeforeShowMakeAsync()
        {
            switch (inputData.tabType)
            {
                case AdminBRO.GachaItem.TabType_OverlordEquipment:
                    haremButton.gameObject.SetActive(false);
                    break;
                case AdminBRO.GachaItem.TabType_Matriachs:
                    haremButtonText.text = "Go to the Harem\nto edit team";
                    overlordButton.gameObject.SetActive(false);
                    break;
                case AdminBRO.GachaItem.TabType_CharactersEquipment:
                    haremButtonText.text = "Go to the Harem\nto equip new weapon";
                    overlordButton.gameObject.SetActive(false);
                    break;
                case AdminBRO.GachaItem.TabType_Shards:
                    haremButtonText.text = "Go to the Harem\nto activate shards";
                    overlordButton.gameObject.SetActive(false);
                    break;
            }

            await Task.CompletedTask;
        }

        private void OverlordButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<OverlordScreen>();
        }

        private void HaremButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<HaremScreen>();
        }

        private void PortalButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

            UIManager.MakeScreen<PortalScreen>().
                SetData(new PortalScreenInData
                {
                    activeButtonId = inputData.tabType switch
                    {
                        AdminBRO.GachaItem.TabType_OverlordEquipment => PortalScreen.TabOverlordEquip,
                        AdminBRO.GachaItem.TabType_CharactersEquipment => PortalScreen.TabBattleGirlsEquip,
                        AdminBRO.GachaItem.TabType_Matriachs => PortalScreen.TabBattleGirls,
                        AdminBRO.GachaItem.TabType_Shards => PortalScreen.TabShards,
                        _ => PortalScreen.TabBattleGirls
                    }
                })
                .RunShowScreenProcess();
        }
    }

    public class SummoningScreenInData : BaseFullScreenInData
    {
        public string tabType;
        public bool isFive;
        public List<AdminBRO.GachaBuyResult> summonData;
    }
}
