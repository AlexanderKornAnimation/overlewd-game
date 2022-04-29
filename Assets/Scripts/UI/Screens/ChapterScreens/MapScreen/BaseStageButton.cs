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
