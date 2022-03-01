using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class ForgeScreen : BaseScreen
    {
        private Button castleBtn;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/ForgeScreen/Forge", transform);

            var canvas = screenInst.transform.Find("Canvas");

            castleBtn = canvas.Find("Castle").GetComponent<Button>();
            castleBtn.onClick.AddListener(CastleButtonClick);
        }

        private void CastleButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI_GenericButtonClick);
            UIManager.ShowScreen<CastleScreen>();
        }
    }
}
