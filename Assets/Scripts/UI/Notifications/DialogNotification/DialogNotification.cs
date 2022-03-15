using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class DialogNotificationMissclick : NotificationMissclickColored
    {
        private IEnumerator EnableByTimer()
        {
            yield return new WaitForSeconds(2.0f);
            missClickEnabled = true;
        }

        public void OnReset()
        {
            missClickEnabled = false;
            StopCoroutine(EnableByTimer());
            StartCoroutine(EnableByTimer());
        }
    }


    public class DialogNotification : BaseNotification
    {
        protected Button button;
        protected TextMeshProUGUI text;
        protected Transform emotionBack;
        protected Transform emotionPos;

        protected SpineWidgetGroup emotionAnimation;
        protected AdminBRO.Dialog dialogData;

        protected virtual void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Notifications/DialogNotification/DialogNotification", transform);

            var canvas = screenInst.transform.Find("Canvas");
 
            button = canvas.Find("Button").GetComponent<Button>();
            button.onClick.AddListener(ButtonClick);

            var banner = canvas.Find("Banner");
            text = banner.Find("Text").GetComponent<TextMeshProUGUI>();
            emotionBack = banner.Find("EmotionBack");
            emotionPos = emotionBack.Find("EmotionPos");
        }

        protected virtual void ButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.HideNotification();

            LeaveByButtonScreen();
        }

        public override void ShowMissclick()
        {
            var missclick = UIManager.ShowNotificationMissclick<DialogNotificationMissclick>();
            missclick.missClickEnabled = false;
        }

        public override async Task BeforeShowAsync()
        {
            EnterScreen();
            Customize();
            StartCoroutine(CloseByTimer());
            await Task.CompletedTask;
        }

        public override async Task BeforeHideAsync()
        {
            StopCoroutine(CloseByTimer());

            await Task.CompletedTask;
        }

        public override void AfterShow()
        {
            UIManager.GetNotificationMissclick<DialogNotificationMissclick>()?.OnReset();
        }

        public override void StartShow()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericDialogNotificationShow);
        }

        public override ScreenShow Show()
        {
            return gameObject.AddComponent<ScreenLeftShow>();
        }

        public override ScreenHide Hide()
        {
            return gameObject.AddComponent<ScreenLeftHide>();
        }

        private void Customize()
        {
            if (dialogData != null)
            {
                var firstReplica = dialogData.replicas.First();
                text.text = firstReplica.message;

                if (firstReplica.emotionAnimationId.HasValue)
                {
                    var animation = GetAnimationById(firstReplica.emotionAnimationId.Value);
                    emotionAnimation = SpineWidgetGroup.GetInstance(emotionPos);
                    emotionAnimation.Initialize(animation);
                }
            }
        }

        private IEnumerator CloseByTimer()
        {
            yield return new WaitForSeconds(4.0f);
            UIManager.HideNotification();

            LeaveByTimerScreen();
        }

        protected virtual void EnterScreen()
        {
            //dialogData = 
        }

        protected virtual void LeaveByTimerScreen()
        {

        }

        protected virtual void LeaveByButtonScreen()
        {

        }

        protected virtual AdminBRO.Animation GetAnimationById(int id)
        {
            return GameData.GetAnimationById(id);
        }
    }
}
