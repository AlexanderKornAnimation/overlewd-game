using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class ForgeScreen : BaseFullScreenParent<ForgeScreenInData>
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
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<CastleScreen>();
        }
    }

    public class ForgeScreenInData : BaseFullScreenInData
    {

    }
}
