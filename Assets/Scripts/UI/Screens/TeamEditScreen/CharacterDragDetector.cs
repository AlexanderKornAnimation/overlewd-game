using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSTeamEditScreen
    {
        public class CharacterDragDetector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,
            IBeginDragHandler, IDragHandler
        {
            public ScrollRect scrollRect { private get; set; }
            public TeamEditScreen screen { private get; set; }

            private bool dragEnable;
            private bool beginDrag;

            private IEnumerator EnableDragTimeout()
            {
                dragEnable = false;
                beginDrag = false;
                yield return new WaitForSeconds(0.5f);

                var dragObj = screen.MakeDragCharacterObject();
                dragObj.transform.position = Input.mousePosition;
                dragEnable = true;
            }

            public void OnPointerDown(PointerEventData eventData)
            {
                StopCoroutine("EnableDragTimeout");
                StartCoroutine("EnableDragTimeout");
            }

            public void OnPointerUp(PointerEventData eventData)
            {
                StopCoroutine("EnableDragTimeout");
                if (!beginDrag)
                {
                    screen.DestroyDragCharacterObject();
                }
            }

            public void OnBeginDrag(PointerEventData eventData)
            {
                beginDrag = true;
                if (dragEnable)
                {
                    screen.chDragObj.transform.position = Input.mousePosition;
                    ExecuteEvents.Execute(screen.chDragObj.dragger.gameObject, eventData, ExecuteEvents.beginDragHandler);
                    eventData.pointerDrag = screen.chDragObj.dragger.gameObject;
                }
                else
                {
                    StopCoroutine("EnableDragTimeout");
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
