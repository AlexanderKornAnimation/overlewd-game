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
                if (GameGlobalStates.dialogNotification_DialogId == 1)
                {
                    GameGlobalStates.dialogNotification_DialogId = 2;
                    UIManager.ShowNotification<DialogNotification>();
                }
                else if (GameGlobalStates.dialogNotification_DialogId == 2)
                {
                    GameGlobalStates.dialogNotification_DialogId = 3;
                    UIManager.ShowNotification<DialogNotification>();
                }
                else if (GameGlobalStates.dialogNotification_DialogId == 3)
                {
                    GameGlobalStates.dialogNotification_DialogId = 4;
                    UIManager.ShowNotification<DialogNotification>();
                }
                else if (GameGlobalStates.dialogNotification_DialogId == 4)
                {
                    GameGlobalStates.dialogNotification_DialogId = 5;
                    UIManager.ShowNotification<DialogNotification>();
                }
                else if (GameGlobalStates.dialogNotification_DialogId == 5)
                {
                    GameGlobalStates.dialogNotification_DialogId = 6;
                    UIManager.ShowNotification<DialogNotification>();
                }
                else if (GameGlobalStates.dialogNotification_DialogId == 6)
                {
                    GameGlobalStates.dialogNotification_DialogId = 7;
                    UIManager.ShowNotification<DialogNotification>();
                }
                else if (GameGlobalStates.dialogNotification_DialogId == 7)
                {
                    GameGlobalStates.dialogNotification_DialogId = 8;
                    UIManager.ShowNotification<DialogNotification>();
                }
                else if (GameGlobalStates.dialogNotification_DialogId == 8)
                {
                    GameGlobalStates.dialogNotification_DialogId = 9;
                    UIManager.ShowNotification<DialogNotification>();
                }
                else if (GameGlobalStates.dialogNotification_DialogId == 9)
                {
                    GameGlobalStates.dialogNotification_DialogId = 10;
                    UIManager.ShowNotification<DialogNotification>();
                }
                else if (GameGlobalStates.dialogNotification_DialogId == 10)
                {
                    GameGlobalStates.dialogNotification_DialogId = 11;
                    UIManager.ShowNotification<DialogNotification>();
                }
                else if (GameGlobalStates.dialogNotification_DialogId == 11)
                {
                    UIManager.HideNotification();
                    UIManager.ShowPopup<PrepareBattlePopup>();
                }
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
        }
    }
}
