using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSTeamEditScreen
    {
        public class SlotTwoDrop : MonoBehaviour, IDropHandler
        {
            public TeamEditScreen screen { private get; set; }
            public void OnDrop(PointerEventData eventData)
            {
                screen.SlotTwoDrop();
            }
        }
    }
}
