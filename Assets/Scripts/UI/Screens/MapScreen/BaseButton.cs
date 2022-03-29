using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMapScreen
    {
        public abstract class BaseButton : MonoBehaviour
        {
            protected Transform canvas;
            protected Button button;
            protected TextMeshProUGUI title;

            protected virtual void Awake()
            {
                canvas = transform.Find("Canvas");
                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
                title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            }

            protected virtual void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            }
        }
    }
}
