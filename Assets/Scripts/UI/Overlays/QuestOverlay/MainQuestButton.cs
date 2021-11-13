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
                var newItem = QuestButton.LoadPrefab(parent);
                newItem.name = nameof(MainQuestButton);
                return newItem.AddComponent<MainQuestButton>();
            }
        }
    }
}
