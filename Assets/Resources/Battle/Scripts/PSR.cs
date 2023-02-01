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

        public float scaler_big = 0.02f, scaler_small = 0.04f;
        public float defaultStat = 0f;

        public void SetupPSR(BattleManagerInData.BattleFlow flow, bool isEnemy)
        {
            switch (flow)
            {
                case BattleManagerInData.BattleFlow.Win:
                    if (isEnemy)
                    {
                        scaler_big = 0f;
                        scaler_small = 0f;
                        defaultStat = -0.05f;
                    }
                    else 
                    {
                        scaler_big *= 2;
                        scaler_small *= 2;
                        accyracy += scaler_big;
                        crit += scaler_small;
                        defaultStat = 0.2f;
                    }
                    break;
                case BattleManagerInData.BattleFlow.Defeat:
                    if (isEnemy)
                    {
                        scaler_big *= 2;
                        scaler_small *= 2;
                        accyracy += scaler_big;
                        crit += scaler_small;
                        defaultStat = 0.2f;
                    }
                    else
                    {
                        scaler_big = 0f;
                        scaler_small = 0f;
                        defaultStat = -0.05f;
                    }
                    break;
                default:
                    break;
            }
        }

        public void Miss()
        {
            accyracy += scaler_big;
            crit += scaler_small/6;
        }
        public void HitEnemy()
        {
            accyracy = defaultStat;
        }
        public void Dodge()
        {
            dodge = defaultStat * 2;
        }
        public void Crit()
        {
            crit = defaultStat;
        }
        public void CritMiss()
        {
            crit += scaler_small;
        }
        public void EffectHit()
        {
            effectProb = defaultStat;
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