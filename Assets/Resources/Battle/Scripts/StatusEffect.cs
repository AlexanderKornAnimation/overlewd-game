using UnityEngine;

namespace Overlewd
{
    public class StatusEffect : MonoBehaviour
    {
        /// <summary>
        /// накидываемый активный эффект добавляется на персонажа
        /// </summary>
        public bool isActive = false;
        public bool buff = true;
        public bool deBuff = false;

        public AdminBRO.CharacterSkill skill; //скармливаем сюда все проверки
        public CharController cc;

        public string effect = "";
        public int duration = 1;
        public float effectAmount = 1f;
        public float dot = 1f;

        public int
            focus_blind, defUp_defDown,
            regen_poison, bless_healBlock,
            immunity, silence, curse;

        public bool stun;

        public float
            defUp_defDown_dot = 0f,
            regen_poison_dot = 0f,
            bless_dot = 0f,
            curse_dot = 0f;

        public string msg => effect switch
        {
            "defense_up" => "<sprite=\"BuffsNDebuffs\" name=\"BuffDefenceUp\"> Defence up",
            "defense_down" => "<sprite=\"BuffsNDebuffs\" name=\"DebuffDefenceDown\"> Defence down",

            "focus" => "<sprite=\"BuffsNDebuffs\" name=\"BuffFocus\"> Focus",
            "blind" => "<sprite=\"BuffsNDebuffs\" name=\"DebuffBlind\"> Blind",

            "regeneration" => "<sprite=\"BuffsNDebuffs\" name=\"BuffRegeneration\"> Regeneration",
            "poison" => "<sprite=\"BuffsNDebuffs\" name=\"DebuffPoison\"> Poison",

            "bless" => "<sprite=\"BuffsNDebuffs\" name=\"BuffBless\"> Bless",
            "heal_block" => "<sprite=\"BuffsNDebuffs\" name=\"DebuffHealBlock\"> Heal Block",

            "silence" => "<sprite=\"BuffsNDebuffs\" name=\"DebuffSilence\"> Silence",
            "immunity" => "<sprite=\"BuffsNDebuffs\" name=\"BuffImmunity\"> Immunity",
            "stun" => "<sprite=\"BuffsNDebuffs\" name=\"DebuffStun\"> Stun",
            "curse" => "<sprite=\"BuffsNDebuffs\" name=\"DebuffCurse\"> Curse",

            "dispel" => "<sprite=\"BuffsNDebuffs\" name=\"BuffDispell\"> Dispell",
            "safeguard" => "<sprite=\"BuffsNDebuffs\" name=\"BuffSafeguard\"> Safeguard",
            _ => ""
        };

        private void Awake()
        {
            cc = transform.GetComponent<CharController>();
        }
    }
}