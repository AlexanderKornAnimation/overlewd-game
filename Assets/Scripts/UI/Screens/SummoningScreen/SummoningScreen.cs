using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class SummoningScreen : BaseFullScreenParent<SummoningScreenInData>
    {
        private Button backButton;
        private Button portalButton;

        private void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/SummoningScreen/SummoningScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            portalButton = canvas.Find("PortalButton").GetComponent<Button>();
            
            backButton.onClick.AddListener(BackButtonClick);
            portalButton.onClick.AddListener(PortalButtonClick);
        }

        private void Start()
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
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
        }

        private void PortalButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<PortalScreen>();
        }
    }

    public class SummoningScreenInData : BaseFullScreenInData
    {

    }
}
