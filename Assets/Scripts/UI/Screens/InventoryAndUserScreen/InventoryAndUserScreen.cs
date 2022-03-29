using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class InventoryAndUserScreen : BaseFullScreen
    {
        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/InventoryAndUserScreen/Inventory", transform);

            var canvas = screenInst.transform.Find("Canvas");
           
            canvas.Find("Castle").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<CastleScreen>();
            });
        }
    }
}
