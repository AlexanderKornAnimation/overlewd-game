using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSSummoningScreen
    {
        public abstract class BaseShardsAnimCtrl : MonoBehaviour
        {
            protected DropEvent dropEvent;

            protected virtual void Awake()
            {
                dropEvent = GetComponent<DropEvent>();
            }

            public bool IsCompleteOpened =>
                dropEvent?.IsComplete ?? true;

            public void SetShardsData(SummoningScreenShardsData shardsData)
            {
                dropEvent.SetShardsData(shardsData);
            }
        }
    }
}