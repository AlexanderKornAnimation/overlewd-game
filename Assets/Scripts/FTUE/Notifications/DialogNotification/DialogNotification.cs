using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace FTUE
    {
        public class NotificationMissclickColored : Overlewd.NotificationMissclickColored
        {
            protected override async Task PrepareShowOperationsAsync()
            {
                StartCoroutine(EnableByTimer());
                await Task.CompletedTask;
            }

            private IEnumerator EnableByTimer()
            {
                yield return new WaitForSeconds(2.0f);
                missClickEnabled = true;
            }
        }

        public class DialogNotification : Overlewd.DialogNotification
        {
            private SpineWidget emotionAnimation;

            protected override void Awake()
            {
                base.Awake();

                button.gameObject.SetActive(false);
            }

            protected override void ShowMissclick()
            {
                var missclick = UIManager.ShowNotificationMissclick<NotificationMissclickColored>();
                missclick.missClickEnabled = false;
            }

            protected override async Task PrepareShowOperationsAsync()
            {
                var dialogData = GameGlobalStates.dialogNotification_DialogData;

                var firstReplica = dialogData.replicas[0];
                text.text = firstReplica.message;

                if (firstReplica.animation != null)
                {
                    if (GameLocalResources.emotionsAnimPath.ContainsKey(firstReplica.characterKey))
                    {
                        var persEmotions = GameLocalResources.emotionsAnimPath[firstReplica.characterKey];
                        if (persEmotions.ContainsKey(firstReplica.animation))
                        {
                            var headPath = persEmotions[firstReplica.animation];
                            if (headPath != null)
                            {
                                emotionAnimation = SpineWidget.GetInstance(emotionPos);
                                emotionAnimation.Initialize(headPath, false);
                                emotionAnimation.PlayAnimation(firstReplica.animation, true);
                            }
                        }
                    }
                }

                StartCoroutine(CloseByTimer());

                await Task.CompletedTask;
            }

            private IEnumerator CloseByTimer()
            {
                yield return new WaitForSeconds(4.0f);
                UIManager.HideNotification();
            }
        }
    }
}
