using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Overlewd
{
    [CreateAssetMenu(fileName = "New Skill", menuName = "ScriptableObjects/Skill")]
    public class Skill : ScriptableObject
    {
        public string skillName;
        public string discription;
        public Sprite battleIco;
        public GameObject vfx = null;
        public GameObject vfxOnTarget = null;
        public float vfxDuration = 0f;
        public AudioClip sfx;
        public bool select;

        public string
            attackAnimationName = "attack1",
            prepairAnimationName = "prepair1";

        public int power; // damage/healpower/buff/curse etc...
        public int manaCost = 0;
        public int amount = 0; //potions any supplies
        public int cooldown = 0; //in rounds
        public int cooldownCount = 0;

        private void Awake()
        {
            if (vfx != null && vfx.GetComponent<VFXManager>() != null)
                vfxDuration = vfx.GetComponent<VFXManager>().duration;
        }

        public void SaveAmount(int am) => amount = am;
        public void SaveCDcount(int cdc) => cooldownCount = cdc;

        public enum AttackType { MELEE, AOE, POTION, BUFF, CURSE, PASSIVE }
        public AttackType attackType = AttackType.MELEE;
    }
}