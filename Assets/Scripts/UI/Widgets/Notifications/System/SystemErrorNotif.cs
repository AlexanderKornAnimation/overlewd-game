using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace Overlewd
{
    public class SystemErrorNotif : BaseSystemNotif
    {
        protected override void Awake()
        {
            base.Awake();
            notifBack.Find("OkButton").GetComponent<Button>().onClick.AddListener(OkBtnClick);
        }

        private void OkBtnClick()
        {
            state = State.Ok;
        }

        public static SystemErrorNotif GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateScreenPrefab<SystemErrorNotif>
                ("Prefabs/UI/Widgets/Notifications/System/SystemError", parent);
        }
    }
}

