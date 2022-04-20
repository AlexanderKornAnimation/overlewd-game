using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSTeamEditScreen
    {
        public class CharacterDragDetector : MonoBehaviour, IPointerDownHandler,
            IBeginDragHandler, IDragHandler
        {
            public ScrollRect scrollRect { private get; set; }
            public TeamEditScreen screen { private get; set; }

            private bool dragEnable;

            private IEnumerator EnableDragTimeout()
            {
                dragEnable = false;
                yield return new WaitForSeconds(1.0f);
                dragEnable = true;
            }

            public void OnPointerDown(PointerEventData eventData)
            {
                StopCoroutine(EnableDragTimeout());
                StartCoroutine(EnableDragTimeout());
            }

            public void OnBeginDrag(PointerEventData eventData)
            {
                if (dragEnable)
                {
                    gameObject.AddComponent<CharacterDrag>();
                }
                else
                {
                    ExecuteEvents.Execute(scrollRect.gameObject, eventData, ExecuteEvents.beginDragHandler);
                    eventData.pointerDrag = scrollRect.gameObject;
                }
            }

            public void OnDrag(PointerEventData eventData)
            {

            }
        }
    }
}
