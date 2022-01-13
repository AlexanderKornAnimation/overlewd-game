using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class ChestPopup : BasePopup
    {
        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/ChestPopup/ChestPopup", transform);
        }

        public override ScreenShow Show()
        {
            return gameObject.AddComponent<ScreenLeftShow>();
        }

        public override ScreenHide Hide()
        {
            return gameObject.AddComponent<ScreenLeftHide>();
        }
    }

}
