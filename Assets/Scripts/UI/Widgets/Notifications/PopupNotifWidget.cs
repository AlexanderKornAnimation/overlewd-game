using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

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

            var seqPop = DOTween.Sequence();
            seqPop.AppendInterval(1.0f);
            seqPop.onComplete = () => PopupNotifManager.PopNotif();
            seqPop.Play();
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
        private static bool incomeForceShow { get; set; } = true;

        public static void PushNotif(PopupNotifWidget.InitSettings nSetting)
        {
            if (incomeForceShow && (notifSettingsQueue.Count == 0))
            {
                InstNotif(nSetting);
            }
            else
            {
                notifSettingsQueue.Add(nSetting);
            }
        }

        public static void PopNotif()
        {
            var nSettings = notifSettingsQueue.FirstOrDefault();
            if (nSettings != null)
            {
                notifSettingsQueue.RemoveAt(0);
                InstNotif(nSettings);
            }
            else
            {
                incomeForceShow = true;
            }
        }

        private static void InstNotif(PopupNotifWidget.InitSettings nSetting)
        {
            var notifInst = PopupNotifWidget.GetInstance(UIManager.systemNotifRoot);
            notifInst.Init(nSetting);
            notifInst.Play();
            incomeForceShow = false;
        }
    }
}

