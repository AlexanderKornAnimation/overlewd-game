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
    public class EpicFailNotif : BaseSystemNotif
    {
        protected override void Awake()
        {
            var canvas = transform.Find("Canvas");
            notifBack = canvas.Find("NotifBack");
            notifBack.Find("OkButton").GetComponent<Button>().onClick.AddListener(OkBtnClick);

            gameObject.SetActive(false);
        }

        protected override void Start()
        {
            
        }

        private void OkBtnClick()
        {
            state = State.Ok;
        }

        public static EpicFailNotif GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateScreenPrefab<EpicFailNotif>
                ("Prefabs/UI/Widgets/Notifications/System/EpicFail", parent);
        }
    }
}

