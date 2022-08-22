using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Overlewd
{
    namespace NSPortalScreen
    {
        public class ContentByTier : BaseContent
        {
            private Button summonFiveButton;
            private TextMeshProUGUI summonButtonText;
            private List<Transform> steps = new List<Transform>();

            protected override void Awake()
            {
                base.Awake();
                
                summonFiveButton = canvas.Find("SummonFiveButton").GetComponent<Button>();
                summonFiveButton.onClick.AddListener(SummonFiveButtonClick);
                summonButtonText = summonFiveButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();

                for (int i = 1; i <= 10; i++)
                {
                    steps.Add(canvas.Find("Steps").Find($"Step{i}"));
                }
            }

            protected virtual void Start()
            {
                Customize();
            }

            protected virtual void Customize()
            {
                
            }

            public void SummonFiveButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.MakeScreen<SummoningScreen>().
                    SetData(new SummoningScreenInData
                {
                    tabType = gachaData?.tabType,
                    prevScreenInData = UIManager.prevScreenInData,
                    isFive = true
                }).RunShowScreenProcess();
            }
            
            public static ContentByTier GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<ContentByTier>
                    ("Prefabs/UI/Screens/PortalScreen/ContentByTier", parent);
            }
        }
    }
}
