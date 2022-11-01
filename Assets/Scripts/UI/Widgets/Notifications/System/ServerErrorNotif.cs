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
    public class ServerErrorNotif : BaseSystemNotif
    {
        protected override void Awake()
        {
            base.Awake();
            notifBack.Find("CancelButton").GetComponent<Button>().onClick.AddListener(CancelBtnClick);
            notifBack.Find("RetryButton").GetComponent<Button>().onClick.AddListener(RetryBtnClick);
        }
        private void CancelBtnClick()
        {
            state = State.Cancel;
        }

        private void RetryBtnClick()
        {
            state = State.Retry;
        }

        public static ServerErrorNotif GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateScreenPrefab<ServerErrorNotif>
                ("Prefabs/UI/Widgets/Notifications/System/ServerError", parent);
        }
    }
}

