using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class StatusEffect : MonoBehaviour
    {
        public string eName = "defense_up";
        public bool isActive = false;
        public int duration = 1;
        public float dot = 1f;
        public float effectAmount = 1f;
        public bool buff = true;
        public bool deBuff = false;
    }
}