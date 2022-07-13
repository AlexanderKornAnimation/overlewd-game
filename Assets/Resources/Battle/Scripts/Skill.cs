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
        [Space(16)]
        [Tooltip("one onTarget FX to all target, use onlyZ with vfxOnTarget")]
        public GameObject vfx = null;
        public GameObject vfxOnSelf = null;
        public GameObject vfxOnTarget = null;
        public GameObject vfxAOETarget = null;
        public float vfxDuration = 0f;
        [Space(8)]
        public float vfxCascadeTimer = 0f; //for ontarget aoe attack
        [Space(20)]
        public bool shake = false;
        public float shakeDuration = 1.5f;
        public float shakeStrength = 10f;
        [Space(16)]
        public string sfx = null;
        [Space(16)]
        public bool potion;


        private void Awake()
        {
            if (vfx != null && vfx.GetComponent<VFXManager>() != null)
                vfxDuration = vfx.GetComponent<VFXManager>().duration;
        }
    }
}