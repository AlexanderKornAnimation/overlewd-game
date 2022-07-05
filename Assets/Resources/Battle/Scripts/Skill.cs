using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Overlewd
{
    [CreateAssetMenu(fileName = "New Skill", menuName = "ScriptableObjects/Skill")]
    public class Skill : ScriptableObject
    {
        public Sprite battleIco;
        public GameObject vfx = null;
        public GameObject vfxOnTarget = null;
        public float vfxDuration = 0f;
        public string sfx = null;

        public bool potion;

        public int damage; // damage/healpower/buff/curse etc...

        private void Awake()
        {
            if (vfx != null && vfx.GetComponent<VFXManager>() != null)
                vfxDuration = vfx.GetComponent<VFXManager>().duration;
        }
    }
}