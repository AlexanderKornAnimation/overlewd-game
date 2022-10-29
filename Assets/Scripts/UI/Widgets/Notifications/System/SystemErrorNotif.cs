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
        public enum State
        {
            Waiting,
            Ok
        }

        private Transform notifBack;
        private TextMeshProUGUI errorName;
        private TextMeshProUGUI errorMessage;

        public State state = State.Waiting;

        public string title
        {
            get => errorName.text;
            set
            {
                errorName.text = value;
            }
        }
        public string message
        {
            get => errorMessage.text;
            set
            {
                errorMessage.text = value;
            }
        }

        void Awake()
        {
            var canvas = transform.Find("Canvas");
            var notifBack = canvas.Find("NotifBack");
            errorName = notifBack.Find("ErrorName").GetComponent<TextMeshProUGUI>();
            errorMessage = notifBack.Find("ErrorMessage").GetComponent<TextMeshProUGUI>();
            notifBack.Find("OkButton").GetComponent<Button>().onClick.AddListener(OkBtnClick);

            gameObject.SetActive(false);
        }

        private void OkBtnClick()
        {
            state = State.Ok;
        }

        public async Task<State> WaitChangeState()
        {
            await UniTask.WaitUntil(() => state != State.Waiting);
            return state;
        }

        public static SystemErrorNotif GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateScreenPrefab<SystemErrorNotif>
                ("Prefabs/UI/Widgets/Notifications/System/SystemError", parent);
        }
    }
}

