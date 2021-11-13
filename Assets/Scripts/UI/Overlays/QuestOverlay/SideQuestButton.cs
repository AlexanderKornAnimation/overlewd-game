using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSQuestOverlay
    {
        public class SideQuestButton : QuestButton
        {
            protected override void Awake()
            {
                base.Awake();
            }

            void Start()
            {
                SideQuestInfo.GetInstance(contentScrollView?.content);
                QuestDescription.GetInstance(contentScrollView?.content);
            }

            public static SideQuestButton GetInstance(Transform parent)
            {
                var newItem = QuestButton.LoadPrefab(parent);
                newItem.name = nameof(SideQuestButton);
                return newItem.AddComponent<SideQuestButton>();
            }
        }
    }
}
