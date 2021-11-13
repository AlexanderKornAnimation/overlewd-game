using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSQuestOverlay
    {
        public class MatriarchQuestButton : QuestButton
        {
            protected override void Awake()
            {
                base.Awake();
            }

            void Start()
            {
                MatriarchQuestInfo.GetInstance(contentScrollView?.content);
                QuestDescription.GetInstance(contentScrollView?.content);
            }

            public static MatriarchQuestButton GetInstance(Transform parent)
            {
                var newItem = QuestButton.LoadPrefab(parent);
                newItem.name = nameof(MatriarchQuestButton);
                return newItem.AddComponent<MatriarchQuestButton>();
            }
        }
    }
}
