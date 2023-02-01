using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class BossEncounterPopup : MonoBehaviour
    {
        [HideInInspector] public BattleManager bm;
        Animator ani;
        public int state = 0;
        void Start()
        {
            ani = GetComponent<Animator>();
        }
        public void StartBattleBtn()
        {
            popupState = EncounterState.START;
            ani.SetTrigger("Start");
        }
        public void LeaveBtn()
        {

        }
        public void CloseBtn()
        {

        }
        public void EndOfCloseAnimation()
        {
            switch (popupState)
            {
                case EncounterState.TRY:
                    break;
                case EncounterState.START:
                    StartCoroutine(bm.NextWave());
                    break;
                case EncounterState.LOSE:
                    break;
                default:
                    break;
            }
        }

        public enum EncounterState { TRY, START, LOSE }
        public EncounterState popupState = EncounterState.TRY;
    }
}