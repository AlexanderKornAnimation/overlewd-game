using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSDialogScreen
    {
        public class DialogCharacter : MonoBehaviour
        {
            private GameObject persImageAdditive;

            void Awake()
            {
                persImageAdditive = transform.Find("PersImageAdditive").gameObject;
                persImageAdditive.SetActive(false);
            }

            public void Select()
            {
                persImageAdditive.SetActive(true);
            }

            public void Deselect()
            {
                persImageAdditive.SetActive(false);
            }

            public static DialogCharacter GetInstance(string prefabPath, Transform parent)
            {
                var character = (GameObject)Instantiate(Resources.Load(prefabPath), parent);
                return character.AddComponent<DialogCharacter>();
            }
        }
    }
}
