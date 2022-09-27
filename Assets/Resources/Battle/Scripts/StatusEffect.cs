using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class StatusEffect : MonoBehaviour
    {
        /// <summary>
        /// накидываемый активный эффект добавляется на персонажа
        /// </summary>
        public bool isActive = false;
        public bool buff = true;
        public bool deBuff = false;

        public string effect = "defense_up";
        
        public int duration = 1;

        public float effectAmount = 1f;
        public float dot = 1f;
        
    }
}