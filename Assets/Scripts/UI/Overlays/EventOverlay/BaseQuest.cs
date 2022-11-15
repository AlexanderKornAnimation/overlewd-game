using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSEventOverlay
    {
        public abstract class BaseQuest : MonoBehaviour
        {
            protected Transform canvas;

            public int eventId { get; set; }
            public AdminBRO.EventItem eventData =>
                GameData.events.GetEventById(eventId);
            public int questId { get; set; }
            public AdminBRO.QuestItem questData =>
                GameData.quests.GetById(questId);
            
            protected async void ClaimClick()
            {
                await GameData.quests.ClaimReward(questId);
                UITools.ClaimRewards(questData?.rewards);
                Customize();
            }

            protected virtual void Customize()
            {

            }
            
            public void SetCanvasActive(bool value)
            {
                if (value != canvas.gameObject.activeSelf)
                {
                    canvas.gameObject.SetActive(value);
                }
            }
        }
    }
}
