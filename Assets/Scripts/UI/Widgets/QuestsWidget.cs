using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class QuestsWidget : BaseWidget
    {
        void Awake()
        {
            transform.Find("Canvas").Find("Quest1").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowOverlay<QuestOverlay>();
            });

            transform.Find("Canvas").Find("Quest2").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowOverlay<QuestOverlay>();
            });
        }

        void Start()
        {
            
        }

        void Update()
        {

        }

        public static QuestsWidget CreateInstance(Transform parent)
        {
            var prefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Widgets/QuestsWidget/QuestWidget"));
            prefab.name = nameof(QuestsWidget);
            var rectTransform = prefab.GetComponent<RectTransform>();
            rectTransform.SetParent(parent, false);
            UIManager.SetStretch(rectTransform);
            return prefab.AddComponent<QuestsWidget>();
        }
    }
}
