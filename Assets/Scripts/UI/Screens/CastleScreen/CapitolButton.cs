using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class CapitolButton : BaseButton
    {
        private Transform notification;

        private void Awake()
        {
            base.Awake();

            notification = transform.Find("CollectCrystalsNotification");
        }        
        
        protected override void ButtonClick()
        {
            
        }
        
        public static CapitolButton GetInstance(Transform parent)
        {
            var newItem = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Screens/CastleScreen/CapitolButton"), parent);
            newItem.name = nameof(CapitolButton);

            return newItem.AddComponent<CapitolButton>();
        }
    }
}
