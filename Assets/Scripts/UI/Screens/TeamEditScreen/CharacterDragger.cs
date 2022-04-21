using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSTeamEditScreen
    {
        public class CharacterDragger: MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
        {
            public CharacterDrag dragObj { get; set; }

            void Awake()
            {

            }

            public void OnBeginDrag(PointerEventData eventData)
            {

            }

            public void OnEndDrag(PointerEventData eventData)
            {
                dragObj.screen.DestroyDragCharacterObject();
            }

            public void OnDrag(PointerEventData eventData)
            {
                var dragObjRT = dragObj.transform as RectTransform;
                dragObjRT.position += new Vector3(eventData.delta.x, eventData.delta.y);
                //dragObjRT.anchoredPosition += eventData.delta / UIManager.GetScreenScaleFactor();
                //dragObjRT.position = Input.mousePosition;
            }
        }
    }
}
