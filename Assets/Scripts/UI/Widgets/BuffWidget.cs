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
        private Button activeButton;
        private Button unactiveButton;

        protected override void Awake()
        {
            var canvas = transform.Find("Canvas");
            backRect = canvas.Find("BackRect").GetComponent<RectTransform>();

            unactiveButton = backRect.Find("UnactiveButton").GetComponent<Button>();
            unactiveButton.onClick.AddListener(ButtonClick);
            
            activeButton = backRect.Find("ActiveButton").GetComponent<Button>();
            activeButton.onClick.AddListener(ButtonClick);
            
            icon = activeButton.transform.Find("Icon").GetComponent<Image>();
            iconUlvi = activeButton.transform.Find("IconUlvi");
            iconAdriel = activeButton.transform.Find("IconAdriel");
            iconIngie = activeButton.transform.Find("IconIngie");
            iconLili = activeButton.transform.Find("IconLili");
            iconFaye = activeButton.transform.Find("IconFaye");
            description = activeButton.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            
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
                activeButton.gameObject.SetActive(true);
                unactiveButton.gameObject.SetActive(false);

                icon.sprite = ResourceManager.LoadSprite(activeBuff.icon);
                description.text = UITools.ChangeTextSize(activeBuff.description, description.fontSize);
                iconUlvi.gameObject.SetActive(activeBuff.matriarch?.isUlvi ?? false);
                iconAdriel.gameObject.SetActive(activeBuff.matriarch?.isAdriel ?? false);
                iconIngie.gameObject.SetActive(activeBuff.matriarch?.isIngie ?? false);
                iconLili.gameObject.SetActive(activeBuff.matriarch?.isLili ?? false);
                iconFaye.gameObject.SetActive(activeBuff.matriarch?.isFaye ?? false);
            }
            else
            {
                activeButton.gameObject.SetActive(false);
                unactiveButton.gameObject.SetActive(true);
            }

            UITools.DisableButton(activeButton, GameData.progressFlags.lockBuff);
            UITools.DisableButton(unactiveButton, GameData.progressFlags.lockBuff);
        }

        protected virtual void ButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakeScreen<HaremScreen>().
                SetData(new HaremScreenInData
                {
                    prevScreenInData = UIManager.prevScreenInData
                }).RunShowScreenProcess();
        }
        
        public static BuffWidget GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateScreenPrefab<BuffWidget>
                ("Prefabs/UI/Widgets/BuffWidget/BuffWidget", parent);
        }
    }
}

