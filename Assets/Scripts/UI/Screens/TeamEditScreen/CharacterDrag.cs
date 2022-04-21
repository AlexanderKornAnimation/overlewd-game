using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSTeamEditScreen
    {
        public class CharacterDrag : MonoBehaviour
        {
            public CharacterDragger dragger { get; private set; }
            public TeamEditScreen screen { get; set; }

            void Awake()
            {
                var canvas = transform.Find("Canvas");
                dragger = canvas.Find("DragDetectorArea").GetComponent<CharacterDragger>();
                dragger.dragObj = this;
            }

            public static CharacterDrag GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<CharacterDrag>(
                    "Prefabs/UI/Screens/TeamEditScreen/CharacterDrag", parent);
            }
        }
    }
}
