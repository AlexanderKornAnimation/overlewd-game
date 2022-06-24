using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class FireballSpell : MonoBehaviour
    {
        public static FireballSpell GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateWidgetPrefab<FireballSpell>("Prefabs/UI/Popups/SpellPopup/FireballImage",
                parent);
        }
    }
}