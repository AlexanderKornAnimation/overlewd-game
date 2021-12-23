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
            protected override void OnClick()
            {
                
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
                UIManager.ShowNotificationMissclick<NotificationMissclickColored>();
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

                await Task.CompletedTask;
            }

            private IEnumerator CloseByTimer()
            {
                yield return new WaitForSeconds(5.0f);
                UIManager.HideNotification();
            }

            protected override async Task AfterShowOperationsAsync()
            {
                StartCoroutine(CloseByTimer());

                await Task.CompletedTask;
            }
        }
    }
}
