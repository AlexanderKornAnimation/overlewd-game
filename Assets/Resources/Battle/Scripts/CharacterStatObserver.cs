using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class CharacterStatObserver : MonoBehaviour
    {

        public CharController cc; //присваивается при создании

        public Transform persPos => cc.persPos;

        //Char UI
        //public Button bt;
        public Slider sliderHP;
        public Slider sliderMP;
        public TextMeshProUGUI hpTMP;
        public TextMeshProUGUI mpTMP;

        public CharacterPortrait charStats;

        public float health => cc.health;
        public float healthMax => cc.healthMax;
        public float mana => cc.mana;
        public float manaMax => cc.manaMax;
        public bool isEnemy => cc.isEnemy;
        public bool isOverlord => cc.isOverlord;

        private int focus_blind => cc.focus_blind;
        private int defUp_defDown => cc.defUp_defDown;
        private int regen_poison => cc.regen_poison;
        private int bless_healBlock => cc.bless_healBlock;
        private int immunity => cc.immunity;
        private int silence => cc.silence;
        private int curse => cc.curse;
        private bool stun => cc.stun; //only ico

        [Header("Text Mesh Pro")]
        [SerializeField]
        private TextMeshProUGUI
            focus_tmp;
        [SerializeField]
        private TextMeshProUGUI
            blind_tmp,
            defUp_tmp,
            defDown_tmp,
            regen_tmp,
            poison_tmp,
            bless_tmp,
            healBlock_tmp,
            immunity_tmp,
            silence_tmp,
            curse_tmp;

        [Header("Icons")]
        [SerializeField]
        private GameObject
            focus_obj;
        [SerializeField]
        private GameObject
            blind_obj,
            defUp_obj,
            defDown_obj,
            regen_obj,
            poison_obj,
            bless_obj,
            healBlock_obj,
            immunity_obj,
            silence_obj,
            curse_obj,
            stun_obj;

        private void Start()
        {
            charStats.InitUI();

            if (sliderHP) sliderHP.maxValue = healthMax;
            if (sliderMP) sliderMP.maxValue = manaMax;

            UpdateUI();
            UpdateStatuses();
        }

        public void UpdateUI()
        {
            string hpTxt = $"{health}/{healthMax}";
            if (hpTMP != null) hpTMP.text = hpTxt; else Debug.Log("hpTMP = null");
            if (sliderHP != null) sliderHP.value = health;
            if (isOverlord)
            {
                string mpTxt = $"{mana}/{manaMax}";
                if (mpTMP != null) mpTMP.text = mpTxt;
                if (sliderMP != null) sliderMP.value = mana;
            }
            charStats.UpdateUI();
        }

        public void UpdateStatuses()
        {
            ApplyStat(focus_blind, focus_obj, focus_tmp);
            ApplyStat(focus_blind, blind_obj, blind_tmp, buff: false);

            ApplyStat(defUp_defDown, defUp_obj, defUp_tmp);
            ApplyStat(defUp_defDown, defDown_obj, defDown_tmp, buff: false);

            ApplyStat(regen_poison, regen_obj, regen_tmp);
            ApplyStat(regen_poison, poison_obj, poison_tmp, buff: false);

            ApplyStat(bless_healBlock, bless_obj, bless_tmp);
            ApplyStat(bless_healBlock, healBlock_obj, healBlock_tmp, buff: false);

            ApplyStat(immunity, immunity_obj, immunity_tmp);
            ApplyStat(curse, curse_obj, curse_tmp);
            ApplyStat(silence, silence_obj, silence_tmp);

            stun_obj.SetActive(stun);
        }

        void ApplyStat(int effect, GameObject icon, TextMeshProUGUI text, bool buff = true)
        {
            if (effect == 0)
                icon.SetActive(false);
            else
            {
                if (buff)
                    icon.SetActive(effect > 0);
                else
                    icon.SetActive(effect < 0);
                text.text = (Mathf.Abs(effect) > 1) ? Mathf.Abs(effect).ToString() : "";
            }
        }
        private void OnGUI()
        {
            if (cc.bm.debug > 0)
            {
                Vector3 pos = cc.persPos.position;
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.black;
                style.fontSize = 18;

                if (cc.bm.debug == 1)
                {
                    bool skills = cc.character.skills != null;

                    //GUI.Box(new Rect(pos.x + 34, pos.y - 156, 184, 404), GUIContent.none);
                    for (int i = 0; i < 3; i++)
                    {
                        if (i == 2)
                            style.normal.textColor = Color.white;

                        GUI.Label(new Rect(pos.x + 40, pos.y - 150 - i * 2, 180, 400),
                            $"Name: {cc.Name}\n" +
                            $"Level: {cc.character.level}\n" +
                            $"Rarity: {cc.character.rarity}\n" +
                            $"Class: {cc.character.characterClass}\n\n" +

                            $"Speed: {cc.speed}\n" +
                            $"Power: {cc.power}\n" +
                            $"Constitution: {cc.constitution}\n" +
                            $"Agility: {cc.agility}\n\n" +

                            $"Accuracy: {cc.accuracy}\n" +
                            $"Dodge: {cc.dodge}\n" +
                            $"Critrate: {cc.critrate}\n\n" +

                            $"Max Health: {cc.character.health}\n" +
                            $"Damage: {cc.character.damage}\n" +
                            $"Max Mana: {cc.character.mana}\n" +
                            $"Key: {cc.character.key}\n" +
                            $"Damage: {cc.character.damage}\n\n", style);
                    }
                }
                else if (cc.bm.debug == 2)
                {
                    //GUI.Box(new Rect(pos.x + 34, pos.y - 156, 184, 404), GUIContent.none);
                    for (int i = 0; i < 3; i++)
                    {
                        if (i == 2)
                            style.normal.textColor = Color.white;

                        var k = 0;
                        foreach (var item in cc.skill)
                        {
                            GUI.Label(new Rect(pos.x + 40, pos.y -150 + k * 180 - i * 2, 180, 480),
                            $"Skill {k} Name: {item.name}\n" +
                            $"Damage Amount: {item.amount}\n" + //scale in % - (amount/100)
                            $"AOE: {item.AOE}   EffectProb: {item.effectProb}\n" +
                            $"isHeal: {item.actionType}\n" +
                            $"Type: {item.type}\n" +
                            $"Effect: {item.effect}", style);
                            k++;
                        }
                        foreach (var item in cc.passiveSkill)
                        {
                            GUI.Label(new Rect(pos.x + 240, pos.y - 150 + k * 180 - i * 2, 180, 480),
                            $"Skill {k} Name: {item.name}\n" +
                            $"Damage Amount: {item.amount}\n" + //scale in % - (amount/100)
                            $"AOE: {item.AOE}   EffectProb: {item.effectProb}\n" +
                            $"isHeal: {item.actionType}\n" +
                            $"Type: {item.type}\n" +
                            $"Effect: {item.effect}", style);
                            k++;
                        }
                    }
                }
            }
        }

    }
}