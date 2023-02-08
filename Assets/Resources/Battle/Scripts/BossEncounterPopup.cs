using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class BossEncounterPopup : MonoBehaviour
    {
        [HideInInspector] public BattleManager bm;
        [SerializeField] private Image portraitIco;
        private Animator ani;

        void Awake() => ani = GetComponent<Animator>();

        [SerializeField] GameObject[] hidenStart;
        [SerializeField] GameObject[] hidenLose;

        public void StartBattleBtn()
        {
            if (popupState == EncounterState.OPEN)
            {
                popupState = EncounterState.START;
                ani.SetTrigger("Close");
            }
        }
        public void SetUp(Sprite img, bool start = false)
        {
            if (portraitIco && img != null)
                portraitIco.sprite = img;
            if (start)
                foreach (var item in hidenStart)
                    item.SetActive(false);
            else
                foreach (var item in hidenLose)
                    item.SetActive(false);
        }
        public void CloseBtn()
        {
            if (popupState == EncounterState.OPEN)
            {
                popupState = EncounterState.LOSE;
                ani.SetTrigger("Close");
            }
        }
        public void OpenTrigger() => popupState = EncounterState.OPEN;

        public void CloseTrigger()
        {
            switch (popupState)
            {
                case EncounterState.START:
                    bm.StartCoroutine(bm.NextWave(0f, true));
                    break;
                case EncounterState.LOSE:
                    //bm.StartCoroutine(bm.WinScreenWithDelay(0f));
                    bm.LoseBattle();
                    Debug.Log("WINNIG (LOOSING BOSS ENCOUNTER)");
                    break;
                default:
                    break;
            }
            Destroy(gameObject);
        }

        public enum EncounterState { CLOSE, OPEN, START, LOSE }
        public EncounterState popupState = EncounterState.CLOSE;
    }
}