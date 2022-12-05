using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class MouseListener : MonoBehaviour
    {
        private bool mousePressed = false;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            if (Input.GetMouseButton(0) && !mousePressed)
            {
                mousePressed = true;
                //Debug.Log($"Down {Input.mousePosition}");
            }
            else if (!Input.GetMouseButton(0) && mousePressed)
            {
                mousePressed = false;
                //Debug.Log($"Up {Input.mousePosition}");
            }
        }
    }
}
