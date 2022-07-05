using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class CharacterStatObserver : MonoBehaviour
    {

        public CharController cc;

        public Transform persPos => cc.persPos;

        //Char UI
        //public Button bt;
        public Image charClass;
        public Slider sliderHP;
        public Slider sliderMP;
        public bool showMP => cc.isOverlord;
        public TextMeshProUGUI hpTMP;
        public TextMeshProUGUI mpTMP;

        public CharacterPortrait charStats;

        public StatusEffects status_bar;

        public float health => cc.health;
        public float healthMax => cc.healthMax;
        public float mana => cc.mana;
        public float manaMax => cc.manaMax;
        public bool isEnemy => cc.isEnemy;
        public bool isOverlord => cc.isOverlord;


        [SerializeField]
        private List<Sprite> classIcons;
        private void Start()
        {
            charStats.InitUI();
            status_bar = transform.Find("status_bar").GetComponent<StatusEffects>();
            status_bar.cc = cc;
            if (sliderHP) sliderHP.maxValue = healthMax;
            if (sliderMP) sliderMP.maxValue = manaMax;
            sliderMP?.gameObject.SetActive(showMP);
            if (charClass) SetClass();
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

        public void UpdateStatuses() => status_bar?.UpdateStatuses();

        void SetClass()
        {
            switch (cc.character.characterClass)
            {
                case AdminBRO.Character.Class_Assassin:
                    charClass.sprite = classIcons[1];
                    break;
                case AdminBRO.Character.Class_Caster:
                    charClass.sprite = classIcons[2];
                    break;
                case AdminBRO.Character.Class_Bruiser:
                    charClass.sprite = classIcons[3];
                    break;
                case AdminBRO.Character.Class_Healer:
                    charClass.sprite = classIcons[4];
                    break;
                case AdminBRO.Character.Class_Tank:
                    charClass.sprite = classIcons[5];
                    break;
                default:
                    charClass.sprite = classIcons[0];
                    break;
            }
            charClass.SetNativeSize();
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