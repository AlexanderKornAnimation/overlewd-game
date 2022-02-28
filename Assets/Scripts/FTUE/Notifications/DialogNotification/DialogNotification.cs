using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace FTUE
    {
        public class NotificationMissclickColored : Overlewd.NotificationMissclickColored
        {
            private IEnumerator EnableByTimer()
            {
                yield return new WaitForSeconds(2.0f);
                missClickEnabled = true;
            }

            public override void OnStart()
            {
                missClickEnabled = false;
                StopCoroutine(EnableByTimer());
                StartCoroutine(EnableByTimer());
            }

            protected override void OnClick()
            {
                if (GameGlobalStates.newFTUE)
                {
                    var notifications = Overlewd.GameData.ftue.chapters[GameGlobalStates.ftueChapterId].
                        dialogs.FindAll(d => GameData.GetDialogById(d.id)?.type == AdminBRO.DialogType.Notification);
                    var nextNotificationIndex = notifications.FindIndex(n => n.key == GameGlobalStates.dialogNotification_StageKey);
                    if (++nextNotificationIndex < notifications.Count)
                    {
                        GameGlobalStates.dialogNotification_StageKey = notifications[nextNotificationIndex].key;
                        UIManager.ShowNotification<DialogNotification>();
                    }
                    else
                    {
                        UIManager.HideNotification();
                        UIManager.ShowScreen<StartingScreen>();
                    }
                }
            }
        }

        public class DialogNotification : Overlewd.DialogNotification
        {
            private SpineWidgetGroup emotionAnimation;

            protected override void Awake()
            {
                base.Awake();

                button.gameObject.SetActive(false);
            }

            public override void ShowMissclick()
            {
                UIManager.ShowNotificationMissclick<NotificationMissclickColored>();
            }

            public override async Task BeforeShowAsync()
            {
                var dialogData = GameGlobalStates.dialogNotification_DialogData;

                var firstReplica = dialogData.replicas.First();
                text.text = firstReplica.message;

                if (firstReplica.emotionAnimationId.HasValue)
                {
                    var animation = GameData.GetAnimationById(firstReplica.emotionAnimationId.Value);
                    emotionAnimation = SpineWidgetGroup.GetInstance(emotionPos);
                    emotionAnimation.Initialize(animation);
                }

                StartCoroutine(CloseByTimer());

                await Task.CompletedTask;
            }

            private IEnumerator CloseByTimer()
            {
                yield return new WaitForSeconds(4.0f);
                UIManager.HideNotification();

                if (GameGlobalStates.newFTUE)
                {
                    UIManager.ShowScreen<StartingScreen>();
                }
            }
        }
    }
}
