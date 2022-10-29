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
        enum State
        {
            Show,
            Hide
        }

        private Transform back;
        private CanvasGroup backCG;
        private Transform progress;

        private Coroutine hideTimerCoroutine;
        private float backAlphaTime;
        private float progressAngle;
        private float backMasterAlpha;
        private State state = State.Show;

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
            backMasterAlpha = 1.0f;
            StartCoroutine(AnimProgress());
        }

        private IEnumerator AnimProgress()
        {
            while (true)
            {
                backCG.alpha = (0.7f + 0.3f * Mathf.Sin((backAlphaTime / 0.9f) * Mathf.PI * 2.0f)) * backMasterAlpha;
                progress.transform.localEulerAngles = new Vector3(0.0f, 0.0f, progressAngle);
                yield return new WaitForSeconds(0.01f);
                backAlphaTime += 0.01f;
                progressAngle += 8.0f;
                backMasterAlpha = Mathf.Clamp01(state == State.Show ? backMasterAlpha + 0.1f : backMasterAlpha - 0.04f);
            }
        }

        public void Show()
        {
            if (hideTimerCoroutine != null)
            {
                StopCoroutine(hideTimerCoroutine);
                hideTimerCoroutine = null;
            }
            state = State.Show;
            backMasterAlpha = 1.0f;
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
            state = State.Hide;
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }

        public static ServerConnectionNotif GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateScreenPrefab<ServerConnectionNotif>
                ("Prefabs/UI/Widgets/Notifications/System/ServerConnection", parent);
        }
    }
}

