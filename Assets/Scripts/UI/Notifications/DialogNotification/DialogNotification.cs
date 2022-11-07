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


    public class DialogNotification : BaseNotificationParent<DialogNotificationInData>
    {
        private Button button;
        private TextMeshProUGUI text;
        private Transform emotionBack;
        private Transform emotionPos;

        private FMODEvent replicaSound;

        protected virtual void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Notifications/DialogNotification/DialogNotification", transform);

            var canvas = screenInst.transform.Find("Canvas");
 
            button = canvas.Find("Button").GetComponent<Button>();
            button.onClick.AddListener(ButtonClick);
            button.gameObject.SetActive(false);

            var banner = canvas.Find("Banner");
            text = banner.Find("Text").GetComponent<TextMeshProUGUI>();
            emotionBack = banner.Find("EmotionBack");
            emotionPos = emotionBack.Find("EmotionPos");
        }

        protected virtual void ButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.HideNotification();
        }

        public override BaseMissclick MakeMissclick()
        {
            var missclick = UIManager.MakeNotificationMissclick<DialogNotificationMissclick>();
            missclick.missClickEnabled = false;
            return missclick;
        }

        public override async Task BeforeShowAsync()
        {
            var dialogData = inputData?.dialogData;

            var firstReplica = dialogData?.replicas.FirstOrDefault();
            text.text = firstReplica?.message;

            var animationData = GameData.animations.GetById(firstReplica?.emotionAnimationId);
            SpineScene.GetInstance(animationData, emotionPos);

            var replicaSoundData = GameData.sounds.GetById(firstReplica?.replicaSoundId);
            replicaSound = SoundManager.GetEventInstance(replicaSoundData?.eventPath, replicaSoundData?.soundBankId);

            StartCoroutine("CloseByTimerOrReplica");

            await Task.CompletedTask;
        }

        public override async Task BeforeHideAsync()
        {
            StopCoroutine("CloseByTimerOrReplica");

            await Task.CompletedTask;
        }

        public override async Task AfterShowAsync()
        {
            if (replicaSound == null)
            {
                UIManager.GetNotificationMissclick<DialogNotificationMissclick>()?.OnReset();
            }

            await Task.CompletedTask;
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

        private IEnumerator CloseByTimerOrReplica()
        {
            yield return new WaitForSeconds(4.0f);

            while (replicaSound?.IsPlaying() ?? false)
            {
                yield return new WaitForSeconds(0.3f);
            }

            UIManager.HideNotification();
        }
    }

    public class DialogNotificationInData : BaseNotificationInData
    {
        public int? dialogId;
        public AdminBRO.Dialog dialogData =>
            GameData.dialogs.GetById(dialogId);
    }
}
