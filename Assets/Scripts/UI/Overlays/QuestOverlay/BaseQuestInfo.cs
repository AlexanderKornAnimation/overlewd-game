using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSQuestOverlay
    {
        public abstract class BaseQuestInfo : MonoBehaviour
        {
            public int? questId { get; set; }
            public AdminBRO.QuestItem questData => GameData.quests.GetById(questId);
        }
    }
}