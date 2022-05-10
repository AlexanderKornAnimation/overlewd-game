using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventMapScreen
    {
        public abstract class BaseStageButton : BaseButton
        {
            public int? stageId { get; set; }
            protected AdminBRO.EventStageItem stageData
            {
                get
                {
                    return stageId.HasValue ? GameData.GetEventStageById(stageId.Value) : null;
                }
            }
            
            protected Transform done;
            protected Transform animPos;
            protected SpineWidget anim;

            protected override void Awake()
            {
                base.Awake();
                done = button.transform.Find("Done");
                animPos = canvas.transform.Find("AnimPos");
            }

            protected virtual void Start()
            {
                Customize();
            }

            protected virtual void Customize()
            {
                done.gameObject.SetActive(stageData.isComplete);
            }
            
            protected void SetAnimation(string prefabPath)
            {
                if (stageData.isComplete)
                    return;
                
                button.gameObject.SetActive(false);
                anim = SpineWidget.GetInstance(prefabPath, animPos);
                anim.completeListeners += RemoveAnimation;
                
                if (anim != null)
                    anim.PlayAnimation("action", false);
            }

            private void RemoveAnimation()
            {
                button.gameObject.SetActive(true);
                Destroy(anim.gameObject);
            }
        }
    }
}

