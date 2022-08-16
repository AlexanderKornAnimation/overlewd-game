using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMemoryListScreen
    {
        public class BaseMemory : MonoBehaviour
        {
            protected Transform canvas;
            protected Button button;
            protected Image art;
            
            public int? memoryId;
            public AdminBRO.MemoryItem memoryData => GameData.matriarchs.GetMemoryById(memoryId);

            protected virtual void Awake()
            {
                canvas = transform.Find("Canvas");
                
                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
                art = button.transform.Find("Art").GetComponent<Image>();
            }

            private void Start()
            {
                Customize();
            }

            protected virtual void Customize()
            {
                
            }
            
            protected virtual void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.MakeScreen<SexScreen>().
                    SetData(new SexScreenInData
                    {
                        dialogId = memoryData?.sexSceneId,
                        prevScreenInData = UIManager.prevScreenInData
                    }).RunShowScreenProcess();
            }
        }
    }
}