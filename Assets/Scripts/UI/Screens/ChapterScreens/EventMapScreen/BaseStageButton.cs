using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSEventMapScreen
    {
        public abstract class BaseStageButton : MonoBehaviour
        {
            public int? stageId { get; set; }
            protected AdminBRO.EventStageItem stageData
            {
                get
                {
                    return stageId.HasValue ? GameData.GetEventStageById(stageId.Value) : null;
                }
            }
        }
    }
}

