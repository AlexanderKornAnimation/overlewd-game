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
        private static List<GameDataEvent> gameDataEventsQuery { get; set; } = new List<GameDataEvent>();
        public static void ThrowGameDataEvent(GameDataEvent eventData)
        {
            if (inTransitionState)
            {
                gameDataEventsQuery.Add(eventData);
            }
            else
            {
                GameDataEventHandling(eventData);
            }
        }

        private static void GameDataEventHandling(GameDataEvent eventData)
        {
            switch (eventData.id)
            {
                case GameDataEventId.WalletStateChange:
                    if (GetWidgets<WalletWidget>().Count == 0)
                    {
                        var notif = WalletChangeNotifWidget.GetInstance(systemNotifRoot);
                        notif.Show();
                    }
                    break;
            }

            screen?.OnGameDataEvent(eventData);
            popup?.OnGameDataEvent(eventData);
            overlay?.OnGameDataEvent(eventData);

            foreach (var w in widgets)
            {
                w.OnGameDataEvent(eventData);
            }
        }

        private static void GameDataEventsQueryHandling()
        {
            var tempQuery = new List<GameDataEvent>(gameDataEventsQuery);
            gameDataEventsQuery.Clear();
            foreach (var e in tempQuery)
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
            switch (eventData.id)
            {
                default:
                    if (!inTransitionState)
                    {
                        GameDataEventsQueryHandling();
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
