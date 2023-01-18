using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using System.Linq;

namespace Overlewd
{
    public static partial class UIManager
    {
        private static List<GameDataEvent> gameDataEventsQueue { get; set; } = new List<GameDataEvent>();
        public static void QueuedGameDataEvent(GameDataEvent eventData)
        {
            gameDataEventsQueue.Add(eventData);
        }
        public static void ThrowGameDataEvent(GameDataEvent eventData)
        {
            if (inTransitionState)
            {
                QueuedGameDataEvent(eventData);
            }
            else
            {
                GameDataEventHandling(eventData);
            }
        }

        private static void GameDataEventHandling(GameDataEvent eventData)
        {
            eventData?.Handle();

            screen?.OnGameDataEvent(eventData);
            popup?.OnGameDataEvent(eventData);
            overlay?.OnGameDataEvent(eventData);

            foreach (var w in widgets)
            {
                w.OnGameDataEvent(eventData);
            }
        }

        private static void GameDataEventsQueueHandling()
        {
            var queue = new List<GameDataEvent>(gameDataEventsQueue);
            gameDataEventsQueue.Clear();
            foreach (var e in queue)
            {
                GameDataEventHandling(e);
            }
        }

        public static void ThrowUIEvent(UIEvent eventData)
        {
            UIEventHandling(eventData);
        }

        private static void UIEventHandling(UIEvent eventData)
        {
            //handling game data events queue
            switch (eventData.id)
            {
                case UIEventId.HideOverlay:
                case UIEventId.ShowOverlay:
                case UIEventId.HidePopup:
                case UIEventId.ShowPopup:
                case UIEventId.ChangeScreenComplete:
                case UIEventId.RestoreStateComplete:
                    if (!inTransitionState)
                    {
                        GameDataEventsQueueHandling();
                    }
                    break;
            }

            screen?.OnUIEvent(eventData);
            popup?.OnUIEvent(eventData);
            overlay?.OnUIEvent(eventData);

            foreach (var w in widgets)
            {
                w.OnUIEvent(eventData);
            }
        }

    }
}
