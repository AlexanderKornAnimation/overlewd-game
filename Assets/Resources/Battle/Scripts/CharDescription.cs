using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class CharDescription : MonoBehaviour
    {
        private CharController cc;
        [SerializeField] private TextMeshProUGUI nameTMP, lvlTMP;
        [SerializeField] private Image portraitIco, classIco;
        [SerializeField] private Sprite[] classIcons;
        [SerializeField] private TextMeshProUGUI accur, dodge, crit, health, damage;
        [SerializeField] private StatusEffects status_bar, status_bar_debuff;
        [SerializeField] private GameObject titleBuffs, titleDebuffs;
        private BattleManager bm => FindObjectOfType<BattleManager>();
        Animator ani;
        RectTransform rt;
        Button btn;
        GameObject statusBarGO;
        public bool isOpen = false;
        private void Awake()
        {
            ani = GetComponent<Animator>();
            rt = GetComponent<RectTransform>();
            btn = GetComponent<Button>();
            btn.onClick.AddListener(Close);
            statusBarGO = transform.Find("Status").gameObject;
        }
        public void Open(CharController charC)
        {
            if (bm.battleState == BattleManager.BattleState.PLAYER)
            {
                cc = charC;
                status_bar.cc = cc;
                status_bar_debuff.cc = cc;
                if (!isOpen) {
                    ChangeStats();
                    ani.SetTrigger("Open");
                }
                else
                    ani.SetTrigger("ReOpen");
                isOpen = true;
            }
        }
        public void ChangeStats()
        {
            bool haveAnyStatus = status_bar.StatusCheck(true);
            bool haveAnyStatusDebuff = status_bar_debuff.StatusCheck(false);
            rt.sizeDelta = (haveAnyStatus || haveAnyStatusDebuff) ? new Vector2(786, 546) : new Vector2(344, 546);
            lvlTMP.text = cc.level.ToString();
            nameTMP.text = cc.Name;

            if (cc.icon != null) portraitIco.sprite = cc.icon;
            if (classIco && classIcons != null) SetClass();

            accur.text = $"{cc.accuracy * 100}%";
            dodge.text = $"{cc.dodge * 100}%";
            crit.text = $"{cc.critrate * 100}%";
            health.text = cc.healthMax.ToString();
            damage.text = cc.damage.ToString();
            status_bar.UpdateStatuses(true);
            status_bar_debuff.UpdateStatuses(true);
            titleBuffs.SetActive(haveAnyStatus);
            titleDebuffs.SetActive(haveAnyStatusDebuff);
            statusBarGO.SetActive(haveAnyStatus || haveAnyStatusDebuff);
        }
        public void Close()
        {
            if (isOpen)
                ani.SetTrigger("Close");
            isOpen = false;
        }

        void SetClass()
        {
            switch (cc.character.characterClass)
            {
                case AdminBRO.CharacterClass.Assassin:
                    classIco.sprite = classIcons[1];
                    break;
                case AdminBRO.CharacterClass.Bruiser:
                    classIco.sprite = classIcons[2];
                    break;
                case AdminBRO.CharacterClass.Caster:
                    classIco.sprite = classIcons[3];
                    break;
                case AdminBRO.CharacterClass.Healer:
                    classIco.sprite = classIcons[4];
                    break;
                case AdminBRO.CharacterClass.Tank:
                    classIco.sprite = classIcons[5];
                    break;
                default:
                    classIco.sprite = classIcons[0];
                    break;
            }
        }
    }
}