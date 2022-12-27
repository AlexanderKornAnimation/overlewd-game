using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Overlewd
{
    public class UIButtonLongPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {

        public UnityEvent longPressAction, releaseAction;

        private bool pressed = false;
        private float pressTimer = .5f, pressTime = 0f;

        private void Update()
        {
            if (pressed)
            {
                if (pressTime < pressTimer)
                {
                    pressTime += Time.deltaTime;
                }
                else
                {
                    longPressAction?.Invoke();
                    pressTime = 0f;
                    pressed = false;
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            eventData.clickTime = 0f;
            pressTime = 0f;
            pressed = true;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            
            releaseAction?.Invoke();
            if (pressed)
            {
                //charDescription.Close();
                //OnClickAction.Invoke();
            }
            pressTime = 0f;
            pressed = false;
        }
    }
}