using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace Overlewd
{
    public class PopupNotifWidget : BaseWidget
    {
        private RectTransform canvasRT;
        private Transform back;
        private CanvasGroup backCG;
        private TextMeshProUGUI title;
        private TextMeshProUGUI entityTitle;

        protected override void Awake()
        {
            var canvas = transform.Find("Canvas");
            canvasRT = canvas.GetComponent<RectTransform>();
            back = canvas.Find("Back");
            backCG = back.GetComponent<CanvasGroup>();
            title = back.Find("Title").GetComponent<TextMeshProUGUI>();
            entityTitle = back.Find("EntityTitle").GetComponent<TextMeshProUGUI>();
        }

        public void Init(InitSettings settings)
        {
            title.text = settings.title;
            entityTitle.text = settings.message;

            backCG.alpha = 0.0f;
            var tPos = back.position;
            tPos.y += canvasRT.rect.height * 0.05f;
            back.position = tPos;
        }

        public void Play()
        {
            var backEndPosY = back.position.y + canvasRT.rect.height * 0.3f;

            var seq = DOTween.Sequence();
            seq.Join(back.DOMoveY(backEndPosY, 2.1f));
            var fadeSeq = DOTween.Sequence();
            fadeSeq.Append(backCG.DOFade(0.9f, 0.3f));
            fadeSeq.AppendInterval(1.3f);
            fadeSeq.Append(backCG.DOFade(0.0f, 0.5f));
            seq.Join(fadeSeq);
            seq.onComplete = () => Destroy(gameObject);
            seq.Play();
        }

        public static PopupNotifWidget GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateScreenPrefab<PopupNotifWidget>
                ("Prefabs/UI/Widgets/Notifications/PopupNotifWidget", parent);
        }

        public class InitSettings
        {
            public string title;
            public string message;
        }
    }

    public static class PopupNotifManager
    {
        private static List<PopupNotifWidget.InitSettings> notifSettingsQueue =
            new List<PopupNotifWidget.InitSettings>();

        private static bool doQueueEnabled { get; set; } = false;
        private static async void DoQueue()
        {
            if (doQueueEnabled)
                return;
            doQueueEnabled = true;
            while (notifSettingsQueue.Count > 0)
            {
                InstNotif(notifSettingsQueue[0]);
                notifSettingsQueue.RemoveAt(0);
                await UniTask.Delay(1000);
            }
            doQueueEnabled = false;
        }

        public static void PushNotif(PopupNotifWidget.InitSettings nSetting)
        {
            notifSettingsQueue.Add(nSetting);
            DoQueue();
        }

        private static void InstNotif(PopupNotifWidget.InitSettings nSetting)
        {
            var notifInst = PopupNotifWidget.GetInstance(UIManager.systemNotifRoot);
            notifInst.Init(nSetting);
            notifInst.Play();
        }
    }
}

