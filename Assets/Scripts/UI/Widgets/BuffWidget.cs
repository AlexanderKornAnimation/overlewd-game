using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class BuffWidget : MonoBehaviour
    {
        private RectTransform backRect;
        private Image icon;
        private Button button;
        private TextMeshProUGUI description;
        private TextMeshProUGUI timer;

        public BaseScreenInData inputData { get; set; }
        
        void Awake()
        {
            var canvas = transform.Find("Canvas");
            backRect = canvas.Find("BackRect").GetComponent<RectTransform>();
            button = backRect.Find("Button").GetComponent<Button>();
            icon = button.transform.Find("Icon").GetComponent<Image>();
            description = button.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            timer = button.transform.Find("Timer").GetComponent<TextMeshProUGUI>();
            
            button.onClick.AddListener(ButtonClick);
        }

        protected virtual void ButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakeScreen<HaremScreen>().
                SetData(new HaremScreenInData
            {
                prevScreenInData = inputData
            }).RunShowScreenProcess();
        }

        public void Show()
        {
            UITools.RightShow(backRect);
        }

        public void Hide()
        {
            UITools.RightHide(backRect);
        }

        public async Task ShowAsync()
        {
            await UITools.RightShowAsync(backRect);
        }

        public async Task HideAsync()
        {
            await UITools.RightHideAsync(backRect);
        }
        
        public static BuffWidget GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateScreenPrefab<BuffWidget>
                ("Prefabs/UI/Widgets/BuffWidget/BuffWidget", parent);
        }
    }
}

