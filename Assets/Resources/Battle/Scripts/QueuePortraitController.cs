using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class QueuePortraitController : MonoBehaviour
    {
        Image image;
        Button btn;
        RectTransform rt;
        public CharController cc;
        Vector2 defaultSize = new Vector2(95, 95);
        Vector2 scaledSize = new Vector2(115, 115);
        float maskScale = 1.15f;
        RectTransform mask;

        /// <summary>
        /// при уничтожении запустить фейд в 0 затем сужение preferredWidth до 0
        /// после чего можно уничтожать (делать быстро)
        /// </summary>

        private void Awake()
        {
            mask = transform.Find("mask").GetComponent<RectTransform>();
            image = transform.Find("mask/Ico").GetComponent<Image>();
            btn = GetComponent<Button>();
            rt = GetComponent<RectTransform>();
        }

        public void SetUp(CharController cc)
        {
            this.cc = cc;
            btn.onClick.AddListener(cc.Select);
            StartCoroutine(SetIcon());
        }
        IEnumerator SetIcon()
        {
            yield return new WaitForEndOfFrame();
            image.sprite = cc.icon;
        }
        public void Select()
        {
            transform.SetSiblingIndex(0);
            rt.sizeDelta = scaledSize;
            mask.localScale *= maskScale;
        }
        public void Deselect()
        {
            var parCount = transform.parent.transform.childCount;
            transform.SetSiblingIndex(parCount);
            rt.sizeDelta = defaultSize;
            mask.localScale = Vector3.one;
        }
        public void DestroyPortrait() => StartCoroutine(DestroyP());

        IEnumerator DestroyP()
        {
            yield return new WaitForSeconds(2f);
            var s = DOTween.Sequence();
            s.Append(transform.GetComponent<CanvasGroup>().DOFade(0, 0.15f));
            s.Append(transform.GetComponent<RectTransform>().DOSizeDelta(new Vector2(0, 0), 0.15f));
            yield return new WaitForSeconds(.3f);
            Destroy(gameObject);
        }
    }
}