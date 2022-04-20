using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSTeamEditScreen
    {
        public class CharacterDrag : MonoBehaviour, IEndDragHandler, IDragHandler
        {
            private RectTransform rectTr;

            void Awake()
            {
                rectTr = GetComponent<RectTransform>();
            }

            public void OnEndDrag(PointerEventData eventData)
            {
                Debug.Log("end drag ch");
                rectTr.anchoredPosition = Vector2.zero;
                Destroy(this);
            }

            public void OnDrag(PointerEventData eventData)
            {
                Debug.Log("drag ch");
                rectTr.position += new Vector3(eventData.delta.x, eventData.delta.y);
                //rectTr.anchoredPosition += eventData.delta / UIManager.GetScreenScaleFactor();
                //rectTr.position = Input.mousePosition;
            }
        }
    }
}
