using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMapScreen
    {
        public abstract class BaseStageButton : BaseButton
        {
            public AdminBRO.FTUEStageItem stageData { get; set; }

            protected Transform done;

            protected override void Awake()
            {
                base.Awake();
                done = button.transform.Find("Done");
            }

            protected virtual void Start()
            {
                done.gameObject.SetActive(stageData?.status == AdminBRO.FTUEStageItem.Status_Complete);
            }
        }
    }
}
