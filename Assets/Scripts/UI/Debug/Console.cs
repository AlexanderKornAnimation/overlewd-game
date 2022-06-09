using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class Console : MonoBehaviour
    {
        private Text text;

        void Awake()
        {
            var canvas = transform.Find("Canvas");
            text = canvas.Find("Text").GetComponent<Text>();
        }

        public void PushMessage(string msg)
        {
            text.text = msg;
        }

        public static Console GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateScreenPrefab<Console>("Prefabs/UI/Debug/Console", parent);
        }
    }
}
