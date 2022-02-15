using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class SummoningScreen : BaseScreen
    {
        private Button backButton;
        private Button haremButton;
        private Button portalButton;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/SummoningScreen/SummoningScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            haremButton = canvas.Find("HaremButton").GetComponent<Button>();
            portalButton = canvas.Find("PortalButton").GetComponent<Button>();
            
            backButton.onClick.AddListener(BackButtonClick);
            haremButton.onClick.AddListener(HaremButtonClick);
            portalButton.onClick.AddListener(PortalButtonClick);
        }

        void Start()
        {
            Customize();
        }

        private void Customize()
        {
            var shardPositions = transform.Find("Canvas").Find("ShardPositions");
            
            for (int i = 1; i <= shardPositions.childCount; i++)
            {
                var pos = shardPositions.Find($"Shard{i}");
                NSSummoningScreen.Shard.GetInstance(pos);
            }
        }
        
        private void BackButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
            UIManager.ShowScreen<CastleScreen>();
        }

        private void HaremButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
            UIManager.ShowScreen<HaremScreen>();
        }

        private void PortalButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
            UIManager.ShowScreen<PortalScreen>();
        }
    }

}
