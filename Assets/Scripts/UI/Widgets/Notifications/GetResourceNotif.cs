using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Overlewd
{
    public class GetResourceNotif : BaseWidget
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

        public void Play(string sTitle, string sEntityTitle)
        {
            title.text = sTitle;
            entityTitle.text = sEntityTitle;

            backCG.alpha = 0.0f;
            var backEndPosY = back.position.y + canvasRT.rect.height * 0.3f;
            var tPos = back.position;
            tPos.y += canvasRT.rect.height * 0.05f;
            back.position = tPos;

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

        public static GetResourceNotif GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateScreenPrefab<GetResourceNotif>
                ("Prefabs/UI/Widgets/Notifications/GetResourceNotif", parent);
        }
    }
}

