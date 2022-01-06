using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSQuestOverlay
    {
        public class MainQuestButton : QuestButton
        {
            
            protected override void Awake()
            {
                base.Awake();
            }

            void Start()
            {
                MainQuestInfo.GetInstance(contentScrollView?.content);
                QuestDescription.GetInstance(contentScrollView?.content);
            }

            public static MainQuestButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<MainQuestButton>
                    ("Prefabs/UI/Overlays/QuestOverlay/QuestButton", parent);
            }
        }
    }
}
