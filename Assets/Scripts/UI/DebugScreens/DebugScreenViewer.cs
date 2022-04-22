using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class DebugScreenViewer : BaseFullScreen
    {
        private List<GameObject> screens = new List<GameObject>();
        private Transform screenPos;

        private Button prevBtn;
        private Button nextBtn;
        private Button castleBtn;

        private int index = 0;

        void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/DebugScreens/DebugContentViewer/DebugScreenViewer",
                    transform);

            var canvas = screenInst.transform.Find("Canvas");

            screenPos = canvas.Find("ScreenPos");

            castleBtn = canvas.Find("Castle").GetComponent<Button>();
            castleBtn.onClick.AddListener(CastleButtonClick);

            prevBtn = canvas.Find("PrevBtn").GetComponent<Button>();
            prevBtn.onClick.AddListener(PrevButtonClick);

            nextBtn = canvas.Find("NextBtn").GetComponent<Button>();
            nextBtn.onClick.AddListener(NextBtnClick);
            
        }

        private void InstScreens()
        {
            screens.Add(ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/BattleGirlScreen/BattleGirlScreen",
                screenPos));
            screens.Add(ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/BuildingScreen/BuildingScreen",
                screenPos));
            screens.Add(ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/CastleScreen/CastleScreen",
                screenPos));
            screens.Add(ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/DialogScreen/DialogScreen",
                screenPos));
            screens.Add(
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/EventMapScreen/EventMap", screenPos));
            screens.Add(ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/EventMarketScreen/EventMarket",
                screenPos));
            screens.Add(ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/GirlScreen/Girl", screenPos));
            screens.Add(ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/HaremScreen/Harem", screenPos));
            screens.Add(ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/MagicGuildScreen/MagicGuild",
                screenPos));
            screens.Add(ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/MapScreen/MapScreen", screenPos));
            screens.Add(ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/MarketScreen/Market", screenPos));
            screens.Add(ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/MemoryListScreen/MemoryListScreen",
                screenPos));
            screens.Add(ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/MemoryScreen/Memory", screenPos));
            screens.Add(ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/PortalScreen/PortalScreen",
                screenPos));
            screens.Add(ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/SexScreen/SexScreen", screenPos));
            screens.Add(ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/SummoningScreen/SummoningScreen",
                screenPos));
            screens.Add(ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/TeamEditScreen/TeamEditScreen",
                screenPos));
            screens.Add(ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/WeaponScreen/WeaponsScreen",
                screenPos));
            //popups
            screens.Add(ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/BuildingPopups/BuildingPopup",
                screenPos));
            screens.Add(
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/DeclinePopup/DeclinePopup", screenPos));
            screens.Add(
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/DefeatPopup/DefeatPopup", screenPos));
            screens.Add(ResourceManager.InstantiateScreenPrefab(
                "Prefabs/UI/Popups/PrepareBattlePopups/PrepareBattlePopup/PrepareBattlePopup", screenPos));
            screens.Add(ResourceManager.InstantiateScreenPrefab(
                "Prefabs/UI/Popups/PrepareBattlePopups/PrepareBossFightPopup/PrepareBossFightPopup", screenPos));
            screens.Add(ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/SpellPopup/SpellPopup", screenPos));
            screens.Add(
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/VictoryPopup/VictoryPopup", screenPos));

            //overlays
            screens.Add(ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Overlays/EventOverlay/EventOverlay", screenPos));
            screens.Add(ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Overlays/QuestOverlay/QuestOverlay", screenPos));
            screens.Add(ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Overlays/SidebarMenuOverlay/SidebarMenuOverlay", screenPos));
            
            for (var i = 1; i < screens.Count; i++)
            {
                var screen = screens[i];
                screen.SetActive(false);
            }
        }

        void Start()
        {
            InstScreens();
        }


        private void CastleButtonClick()
        {
            UIManager.ShowScreen<CastleScreen>();
        }

        private void PrevButtonClick()
        {
            Debug.Log("prev");
            
            if (index == 0)
            {
                screens[index].SetActive(false);
                index = screens.Count - 1;
                screens[index].SetActive(true);
            }
            
            screens[index].SetActive(false);
            index -= 1;
            screens[index].SetActive(true);
        }

        private void NextBtnClick()
        {
            Debug.Log("next");
            if (index == screens.Count - 1)
            {
                screens[index].SetActive(false);
                index = 0;
                screens[index].SetActive(true);
            }
            screens[index].SetActive(false);
            index += 1;
            screens[index].SetActive(true);
        }

        // void OnGUI()
        // {
        //     float offset = Screen.width * 0.05f;
        //     float size = Screen.width * 0.1f;
        //     var rect = new Rect(offset, offset, size, size);
        //
        //     float offsetY = Screen.width * 0.2f;
        //     var rect2 = new Rect(offset, offsetY, size, size);
        //     if (GUI.Button(rect, "Castle"))
        //     {
        //         UIManager.ShowScreen<CastleScreen>();
        //     }
        //
        //     if (GUI.Button(rect2, "ShowScreen"))
        //     {
        //         UIManager.ShowScreen<BattleGirlScreen>();
        //     }
        //
        //     float botOffset = Screen.width * 0.05f;
        //     float botSize = Screen.width * 0.1f;
        //     var botRect = new Rect(botOffset, botOffset, botSize, botSize);
        //     if (GUI.Button(botRect, "NextScreen"))
        //     {
        //         transform.SetSiblingIndex(0);
        //         if (index >= 0)
        //         {
        //             var prevIndex = index;
        //             index += 1;
        //             if (index == screens.Count - 1)
        //             {
        //                 index = 0;
        //                 prevIndex = 0;
        //             }
        //             screens[prevIndex].SetActive(false);
        //             screens[index].SetActive(true);
        //         }
        //     }
        // }
    }
}