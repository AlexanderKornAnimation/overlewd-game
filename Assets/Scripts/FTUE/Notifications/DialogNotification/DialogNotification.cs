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
            void Start()
            {
                StartCoroutine(EnableByTimer());
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

            public override void ShowMissclick()
            {
                var missclick = UIManager.ShowNotificationMissclick<NotificationMissclickColored>();
                missclick.missClickEnabled = false;
            }

            public override async Task BeforeShowAsync()
            {
                var dialogData = GameGlobalStates.dialogNotification_DialogData;

                var firstReplica = dialogData.replicas[0];
                text.text = firstReplica.message;

                if (firstReplica.emotionAnimationTemp != null)
                {
                    if (GameLocalResources.emotionsAnimPath.ContainsKey(firstReplica.characterSkin))
                    {
                        var persEmotions = GameLocalResources.emotionsAnimPath[firstReplica.characterSkin];
                        if (persEmotions.ContainsKey(firstReplica.emotionAnimationTemp))
                        {
                            var headPath = persEmotions[firstReplica.emotionAnimationTemp];
                            if (headPath != null)
                            {
                                emotionAnimation = SpineWidget.GetInstance(emotionPos);
                                emotionAnimation.Initialize(headPath);
                                emotionAnimation.PlayAnimation(firstReplica.emotionAnimationTemp, true);
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
