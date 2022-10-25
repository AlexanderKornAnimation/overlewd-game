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
        

        private void Awake()
        {
            image = transform.Find("Ico").GetComponent<Image>();
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
            rt.localScale *= 1.2f;
        }
        public void Deselect()
        {
            var parCount = transform.parent.transform.childCount;
            transform.SetSiblingIndex(parCount);
            rt.localScale = Vector3.one;
        }
    }
}