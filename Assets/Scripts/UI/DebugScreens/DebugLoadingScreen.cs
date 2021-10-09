using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class DebugLoadingScreen : BaseScreen
    {
        private string loadingLabel;
        private Texture2D screenTexture;

        void Awake()
        {
            screenTexture = Resources.Load<Texture2D>("Ulvi");
        }

        void Start()
        {
            /*StartCoroutine(Player.GetPlayerInfo(e =>
            {
                StartCoroutine(Player.ChangeName("NewName", e =>
                {

                }));
            }));*/

            StartCoroutine(ResourceManager.GetServerResourcesMeta(serverResourcesMeta =>
            {
                if (!ResourceManager.HasFreeSpaceForNewResources(serverResourcesMeta))
                {
                    UIManager.ShowDialogBox("Not enough free space", "", () => Game.Quit());
                }
                else
                {
                    ResourceManager.SaveLocalResourcesMeta(serverResourcesMeta);
                    ResourceManager.runtimeResourcesMeta = serverResourcesMeta;

                    StartCoroutine(ResourceManager.ActualizeResources(
                        serverResourcesMeta,
                        (resourceItemMeta) =>
                        {
                            loadingLabel = "Download: " + resourceItemMeta.url;
                        },
                        () =>
                        {
                            UIManager.ShowScreen<CastleScreen>();
                        }
                    ));
                }
            },
            (errorMsg) =>
            {
                UIManager.ShowDialogBox("Server error", errorMsg, () => Game.Quit());
            }));
        }

        void OnGUI()
        {
            GUI.depth = 2;
            var rect = new Rect(0, 0, Screen.width, Screen.height);
            GUI.DrawTexture(rect, screenTexture);

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontSize = (int)(Screen.height * 0.08);
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.normal.textColor = Color.black;
            int labelHeight = (int)(labelStyle.fontSize * 1.5);
            GUI.Label(new Rect(0, Screen.height - labelHeight, Screen.width, labelHeight), loadingLabel, labelStyle);
        }

        public override void Show()
        {
            gameObject.AddComponent<ImmediatelyShow>();
        }

        public override void Hide()
        {
            gameObject.AddComponent<ImmediatelyHide>();
        }
    }
}
