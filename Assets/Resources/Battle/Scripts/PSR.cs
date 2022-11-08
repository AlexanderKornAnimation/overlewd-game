using UnityEngine;

namespace Overlewd
{
    public class PSR : MonoBehaviour
    {
        public int move = 0;
        public float accyracy { get; private set; } = 0f;
        public float crit { get; private set; } = 0f;
        public float dodge { get; private set; } = 0f;
        public float effectProb { get; private set; } = 0f;

        public float scaler_big = 0.125f, scaler_small = 0.06f;
        
        public void Miss()
        {
            accyracy += scaler_big;
            crit += scaler_small/6;
        }
        public void HitEnemy()
        {
            accyracy = 0f;
        }
        public void Dodge()
        {
            dodge = 0f;
        }
        public void Crit()
        {
            crit = 0f;
        }
        public void CritMiss()
        {
            crit += scaler_small;
        }
        public void EffectHit()
        {
            effectProb = 0f;
        }
        public void EffectMiss()
        {
            effectProb += scaler_small;
        }
        public void TakeDamage()
        {
            accyracy += scaler_big/5;
            crit += scaler_small/3;
            dodge += scaler_small;
        }
        
    }
}