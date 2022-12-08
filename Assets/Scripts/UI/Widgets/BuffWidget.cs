using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class BuffWidget : BaseWidget
    {
        private RectTransform backRect;
        private Image icon;
        private Transform iconUlvi;
        private Transform iconAdriel;
        private Transform iconIngie;
        private Transform iconLili;
        private Transform iconFaye;
        private TextMeshProUGUI description;
        private Button button;

        protected override void Awake()
        {
            var canvas = transform.Find("Canvas");
            backRect = canvas.Find("BackRect").GetComponent<RectTransform>();
            
            button = backRect.Find("Button").GetComponent<Button>();
            button.onClick.AddListener(ButtonClick);
            
            icon = button.transform.Find("Icon").GetComponent<Image>();
            iconUlvi = button.transform.Find("IconUlvi");
            iconAdriel = button.transform.Find("IconAdriel");
            iconIngie = button.transform.Find("IconIngie");
            iconLili = button.transform.Find("IconLili");
            iconFaye = button.transform.Find("IconFaye");
            description = button.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            
        }

        void Start()
        {
            Customize();
        }

        private void Customize()
        {
            var activeBuff = GameData.matriarchs.activeBuff;
            if (activeBuff != null)
            {
                Show();

                icon.sprite = ResourceManager.LoadSprite(activeBuff.icon);
                description.text = UITools.IncNumberSize(activeBuff.description, description.fontSize);
                iconUlvi.gameObject.SetActive(activeBuff.matriarch?.isUlvi ?? false);
                iconAdriel.gameObject.SetActive(activeBuff.matriarch?.isAdriel ?? false);
                iconIngie.gameObject.SetActive(activeBuff.matriarch?.isIngie ?? false);
                iconLili.gameObject.SetActive(activeBuff.matriarch?.isLili ?? false);
                iconFaye.gameObject.SetActive(activeBuff.matriarch?.isFaye ?? false);
            }
            else
            {
                Hide();
            }

            //button.interactable = !GameData.progressFlags.lockBuff;
            button.interactable = false;
        }

        protected virtual void ButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<HaremScreen>();
        }

        public void Show()
        {
            UITools.RightShow(backRect);
        }

        public void Hide()
        {
            UITools.RightHide(backRect);
        }

        public static BuffWidget GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateScreenPrefab<BuffWidget>
                ("Prefabs/UI/Widgets/BuffWidget/BuffWidget", parent);
        }
    }
}

