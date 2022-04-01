using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    [CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Item")]
    public class Item : ScriptableObject
    {
        public int itemID = 0;
        public string itemName = "";
        [TextArea(2, 10)]
        public string discription = "";
        public Sprite ico;
        public int attack = 0;
        public int defence = 0;
    }
}