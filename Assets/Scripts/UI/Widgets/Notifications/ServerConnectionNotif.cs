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
        private CanvasGroup backCG;
        private Transform progress;

        private Coroutine hideTimerCoroutine;
        private float backAlphaTime;
        private float progressAngle;

        void Awake()
        {
            var canvas = transform.Find("Canvas");
            back = canvas.Find("Back");
            backCG = back.GetComponent<CanvasGroup>();
            progress = back.Find("Progress");
        }

        void Start()
        {
            backCG.alpha = 0.6f;
            StartCoroutine(AnimProgress());
        }

        private IEnumerator AnimProgress()
        {
            while (true)
            {
                backCG.alpha = 0.6f + 0.4f * Mathf.Sin((backAlphaTime / 1.5f) * Mathf.PI * 2.0f);
                progress.transform.localEulerAngles = new Vector3(0.0f, 0.0f, progressAngle);
                yield return new WaitForSeconds(0.01f);
                backAlphaTime += 0.01f;
                progressAngle += 5.0f;
            }
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
            yield return new WaitForSeconds(2.5f);
            Destroy(gameObject);
        }

        public static ServerConnectionNotif GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateScreenPrefab<ServerConnectionNotif>
                ("Prefabs/UI/Widgets/Notifications/ServerConnectionNotif", parent);
        }
    }
}

