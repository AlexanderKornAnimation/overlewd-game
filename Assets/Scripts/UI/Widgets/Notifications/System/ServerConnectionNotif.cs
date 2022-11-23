using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Overlewd
{
    public class ServerConnectionNotif : MonoBehaviour
    {
        private Transform back;
        private TextMeshProUGUI title;
        private Transform progress;

        private Coroutine hideTimerCoroutine;

        private Sequence titleSeq;
        private Sequence progressSeq;

        void Awake()
        {
            back = transform.Find("Back");
            title = back.Find("Title").GetComponent<TextMeshProUGUI>();
            progress = back.Find("Progress");
        }

        void Start()
        {
            titleSeq = DOTween.Sequence();
            titleSeq.AppendCallback(() => title.text = "Connecting");
            titleSeq.AppendInterval(0.4f);
            titleSeq.AppendCallback(() => title.text = "Connecting.");
            titleSeq.AppendInterval(0.4f);
            titleSeq.AppendCallback(() => title.text = "Connecting..");
            titleSeq.AppendInterval(0.4f);
            titleSeq.AppendCallback(() => title.text = "Connecting...");
            titleSeq.AppendInterval(0.4f);
            titleSeq.SetLoops(-1);
            titleSeq.Play();

            progressSeq  = DOTween.Sequence();
            progressSeq.Append(progress.transform.DOLocalRotateQuaternion(Quaternion.Euler(0.0f, 0.0f, 120.0f), 0.2f).SetEase(Ease.Linear));
            progressSeq.Append(progress.transform.DOLocalRotateQuaternion(Quaternion.Euler(0.0f, 0.0f, 240.0f), 0.2f).SetEase(Ease.Linear));
            progressSeq.Append(progress.transform.DOLocalRotateQuaternion(Quaternion.Euler(0.0f, 0.0f, 360.0f), 0.2f).SetEase(Ease.Linear));
            progressSeq.SetLoops(-1);
            progressSeq.Play();
        }

        public void Show()
        {
            if (hideTimerCoroutine != null)
            {
                StopCoroutine(hideTimerCoroutine);
                hideTimerCoroutine = null;
            }
        }

        public void Hide()
        {
            if (hideTimerCoroutine == null)
            {
                hideTimerCoroutine = StartCoroutine(HideTimer());
            }
        }

        private IEnumerator HideTimer()
        {
            yield return new WaitForSeconds(0.8f);
            titleSeq.Kill();
            progressSeq.Kill();
            Destroy(gameObject);
        }

        public static ServerConnectionNotif GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateScreenPrefab<ServerConnectionNotif>
                ("Prefabs/UI/Widgets/Notifications/System/ServerConnection", parent);
        }
    }
}

