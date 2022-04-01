using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class SummoningScreen : BaseFullScreen
    {
        protected Button backButton;
        protected Button haremButton;
        protected Button portalButton;

        protected virtual void Awake()
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

        protected virtual void Start()
        {
            Customize();
        }

        protected virtual void Customize()
        {
            var shardPositions = transform.Find("Canvas").Find("ShardPositions");
            
            for (int i = 1; i <= shardPositions.childCount; i++)
            {
                var pos = shardPositions.Find($"Shard{i}");
                NSSummoningScreen.Shard.GetInstance(pos);
            }
        }
        
        protected virtual void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<CastleScreen>();
        }

        protected virtual void HaremButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<HaremScreen>();
        }

        protected virtual void PortalButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<PortalScreen>();
        }
    }
}
