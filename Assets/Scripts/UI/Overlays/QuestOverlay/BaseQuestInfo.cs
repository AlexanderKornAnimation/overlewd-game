using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSQuestOverlay
    {
        public abstract class BaseQuestInfo : MonoBehaviour
        {
            public QuestContentScrollView questContentScrollView { get; set; }

            public AdminBRO.QuestItem questData =>
                questContentScrollView.questButton.questData;
        }
    }
}