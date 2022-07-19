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
            protected bool canvasActive = true;

            public int eventId { get; set; }
            public AdminBRO.EventItem eventData =>
                GameData.events.GetEventById(eventId);
            public int questId { get; set; }
            public AdminBRO.QuestItem questData =>
                GameData.quests.GetById(questId);
            
            protected virtual async void ClaimClick()
            {
                await GameData.quests.ClaimReward(questId);
            }
            
            public virtual void SetCanvasActive(bool value)
            {
                if (value != canvasActive)
                {
                    canvasActive = value;
                    canvas.gameObject.SetActive(canvasActive);
                }
            }
        }
    }
}
