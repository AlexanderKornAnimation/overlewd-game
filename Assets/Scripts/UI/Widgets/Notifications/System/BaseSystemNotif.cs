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
    public abstract class BaseSystemNotif : MonoBehaviour
    {
        public enum State
        {
            Waiting,
            Ok,
            Cancel,
            Retry
        }

        protected Transform notifBack;
        private TextMeshProUGUI _title;
        private TextMeshProUGUI _message;
        private VerticalLayoutGroup scrollContent_vlg;
        private ContentSizeFitter scrollContent_sf;

        public State state { get; protected set; } = State.Waiting;
        public string title
        {
            get => _title.text;
            set
            {
                _title.text = value;
            }
        }
        public string message
        {
            get => _message.text;
            set
            {
                _message.text = value;
            }
        }

        protected virtual void Awake()
        {
            var canvas = transform.Find("Canvas");
            notifBack = canvas.Find("NotifBack");
            _title = notifBack.Find("Title").GetComponent<TextMeshProUGUI>();
            _message = notifBack.Find("MessageScrollView/Viewport/Content/Message").GetComponent<TextMeshProUGUI>();

            scrollContent_sf = notifBack.Find("MessageScrollView/Viewport/Content").GetComponent<ContentSizeFitter>();
            scrollContent_vlg = notifBack.Find("MessageScrollView/Viewport/Content").GetComponent<VerticalLayoutGroup>();

            gameObject.SetActive(false);
        }

        protected virtual void Start()
        {
            // for recalc scroll rect params
            scrollContent_sf.enabled = false;
            scrollContent_vlg.enabled = false;
            scrollContent_sf.enabled = true;
            scrollContent_vlg.enabled = true;
        }

        public async Task<State> WaitChangeState()
        {
            await UniTask.WaitUntil(() => state != State.Waiting);
            return state;
        }

        public async void Show()
        {
            await ShowAsync();
        }

        public async Task ShowAsync()
        {
            gameObject.SetActive(true);
            UIManager.SetUserInputLockerMode(UIManager.UserInputLockerMode.Manual, false);
            await Task.CompletedTask;
        }

        public async void Hide()
        {
            await HideAsync();
        }

        public async Task HideAsync()
        {
            gameObject.SetActive(false);
            UIManager.SetUserInputLockerMode(UIManager.UserInputLockerMode.Auto);
            await Task.CompletedTask;
        }

        public async void Close()
        {
            await CloseAsync();
        }

        public async Task CloseAsync()
        {
            await HideAsync();
            DestroyImmediate(gameObject);
            UIManager.PeakSystemNotif();
        }

        public static T GetInstance<T>(Transform parent) where T : BaseSystemNotif
        {
            return typeof(T).GetMethod("GetInstance").Invoke(null, new[] { parent }) as T;
        }
    }
}

