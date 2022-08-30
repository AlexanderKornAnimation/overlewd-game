using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Overlewd
{
    public class StatusEffects : MonoBehaviour
    {

        [HideInInspector]
        public CharController cc;
        private int focus_blind => cc.focus_blind;
        private int defUp_defDown => cc.defUp_defDown;
        private int regen_poison => cc.regen_poison;
        private int bless_healBlock => cc.bless_healBlock;
        private int immunity => cc.immunity;
        private int silence => cc.silence;
        private int curse => cc.curse;
        private bool stun => cc.stun; //only ico

        [HideInInspector]
        public bool withDescription = false;

        private TextMeshProUGUI
            focus_tmp, blind_tmp,
            defUp_tmp, defDown_tmp,
            regen_tmp, poison_tmp,
            bless_tmp, healBlock_tmp,
            immunity_tmp,
            silence_tmp,
            curse_tmp;

        private Transform
            focus_obj, blind_obj,
            defUp_obj, defDown_obj,
            regen_obj, poison_obj,
            bless_obj, healBlock_obj,
            immunity_obj,
            silence_obj,
            curse_obj,
            stun_obj;

        private void Awake()
        {

            focus_obj = transform.Find("focus");
            blind_obj = transform.Find("blind");
            defUp_obj = transform.Find("defense_up");
            defDown_obj = transform.Find("defense_down");
            regen_obj = transform.Find("regeneration");
            poison_obj = transform.Find("poison");
            bless_obj = transform.Find("bless");
            healBlock_obj = transform.Find("heal_block");
            immunity_obj = transform.Find("immunity");
            silence_obj = transform.Find("silence");
            curse_obj = transform.Find("curse");
            stun_obj = transform.Find("stun");

            focus_tmp = focus_obj.transform.Find("text").GetComponent<TextMeshProUGUI>();
            blind_tmp = blind_obj.transform.Find("text").GetComponent<TextMeshProUGUI>();
            defUp_tmp = defUp_obj.transform.Find("text").GetComponent<TextMeshProUGUI>();
            defDown_tmp = defDown_obj.transform.Find("text").GetComponent<TextMeshProUGUI>();
            regen_tmp = regen_obj.transform.Find("text").GetComponent<TextMeshProUGUI>();
            poison_tmp = poison_obj.transform.Find("text").GetComponent<TextMeshProUGUI>();
            bless_tmp = bless_obj.transform.Find("text").GetComponent<TextMeshProUGUI>();
            healBlock_tmp = healBlock_obj.transform.Find("text").GetComponent<TextMeshProUGUI>();
            immunity_tmp = immunity_obj.transform.Find("text").GetComponent<TextMeshProUGUI>();
            silence_tmp = silence_obj.transform.Find("text").GetComponent<TextMeshProUGUI>();
            curse_tmp = curse_obj.transform.Find("text").GetComponent<TextMeshProUGUI>();

        }
        private void Start()
        {
            if (withDescription)
                foreach (Transform item in transform) //init skill description text field
                {
                    var descriptionTMP = item.Find("description").GetComponent<TextMeshProUGUI>();
                    descriptionTMP.text = GameData.characters.effects.Find(e => e.name == item.name).description;
                }
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

            stun_obj.gameObject.SetActive(stun);
        }
        public bool StatusCheck()
        {
            if (focus_blind == 0 && defUp_defDown == 0
                && bless_healBlock == 0 && regen_poison == 0
                && immunity == 0 && curse == 0 && silence == 0 && !stun
                )
                return false;
            else
                return true;
        }
        void ApplyStat(int effect, Transform icon, TextMeshProUGUI text, bool buff = true)
        {
            if (effect == 0)
                icon.gameObject.SetActive(false);
            else
            {
                if (buff)
                    icon.gameObject.SetActive(effect > 0);
                else
                    icon.gameObject.SetActive(effect < 0);
                text.text = (Mathf.Abs(effect) > 1) ? Mathf.Abs(effect).ToString() : "";
            }
        }
    }
}