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
        private List<NSSummoningScreen.BaseShard> shards = new List<NSSummoningScreen.BaseShard>();

        private Button haremButton;
        private TextMeshProUGUI haremButtonText;
        private Button overlordButton;
        private Button portalButton;
        private TextMeshProUGUI portalButtonText;
        private Transform canvas;

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
        }

        private NSSummoningScreen.BaseShard GetShard(Transform pos)
        {
            return inputData.tabType switch
            {
                AdminBRO.GachaItem.TabType_CharactersEquipment => NSSummoningScreen.Equip.GetInsance(pos),
                AdminBRO.GachaItem.TabType_OverlordEquipment => NSSummoningScreen.Equip.GetInsance(pos),
                AdminBRO.GachaItem.TabType_Matriachs => NSSummoningScreen.BattleGirl.GetInsance(pos),
                AdminBRO.GachaItem.TabType_Shards => NSSummoningScreen.Crystal.GetInstance(pos),
                _ => null
            };
        }

        public override async Task AfterShowAsync()
        {
            if (inputData.isFive)
            {
                SoundManager.PlayOneShot(FMODEventPath.Gacha_x10_open);

                var itemsStartPos = canvas.Find("ItemStartPositions");
                var itemsEndPos = canvas.Find("ItemEndPositions");

                for (int i = 1; i <= 5; i++)
                {
                    var startPos = itemsStartPos.Find($"Item{i}");
                    var endPos = itemsEndPos.Find($"Item{i}").position;

                    var shard = GetShard(startPos);
                    shard.endPos = endPos;
                    shard.tabType = inputData.tabType;

                    await shard.ShowAsync();
                    shards.Add(shard);
                }

                await Task.Delay(1000);
                foreach (var shard in shards)
                {
                    await shard.OpenAsync();
                }
            }
            else
            {
                SoundManager.PlayOneShot(FMODEventPath.Gacha_x1_open);

                var itemSingleStartPos = canvas.Find("ItemSingleStartPos");
                var itemSingleEndPos = canvas.Find("ItemSingleEndPos").position;

                var shard = GetShard(itemSingleStartPos);
                shard.endPos = itemSingleEndPos;
                shard.tabType = inputData.tabType;

                await shard.ShowAsync();
                await Task.Delay(1000);
                await shard.OpenAsync();
                shards.Add(shard);
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
    }
}
