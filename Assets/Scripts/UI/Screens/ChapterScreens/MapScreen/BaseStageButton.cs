using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMapScreen
    {
        public abstract class BaseStageButton : BaseButton
        {
            public int? stageId { get; set; }

            protected AdminBRO.FTUEStageItem stageData
            {
                get
                {
                    return stageId.HasValue ? GameData.GetFTUEStageById(stageId.Value) : null;
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
                done.gameObject.SetActive(stageData.status == AdminBRO.FTUEStageItem.Status_Complete);
                if (stageData.status != AdminBRO.FTUEStageItem.Status_Complete)
                {
                    button.gameObject.SetActive(false);
                    anim = SpineWidget.GetInstance(animPos);
                    anim.completeListeners += RemoveAnimation;
                }
            }

            private void RemoveAnimation()
            {
                button.gameObject.SetActive(true);
                Destroy(anim.gameObject);
            }
        }
    }
}