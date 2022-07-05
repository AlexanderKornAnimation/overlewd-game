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
            focus_obj     = transform.Find("focus");
            blind_obj     = transform.Find("blind");
            defUp_obj     = transform.Find("defUp");
            defDown_obj   = transform.Find("defDown");
            regen_obj     = transform.Find("regen");
            poison_obj    = transform.Find("poison");
            bless_obj     = transform.Find("bless");
            healBlock_obj = transform.Find("healBlock");
            immunity_obj  = transform.Find("immunity");
            silence_obj   = transform.Find("silence");
            curse_obj     = transform.Find("curse");
            stun_obj      = transform.Find("stun");

            focus_tmp     = focus_obj.GetComponentInChildren<TextMeshProUGUI>();
            blind_tmp     = blind_obj.GetComponentInChildren<TextMeshProUGUI>();
            defUp_tmp     = defUp_obj.GetComponentInChildren<TextMeshProUGUI>();
            defDown_tmp   = defDown_obj.GetComponentInChildren<TextMeshProUGUI>();
            regen_tmp     = regen_obj.GetComponentInChildren<TextMeshProUGUI>();
            poison_tmp    = poison_obj.GetComponentInChildren<TextMeshProUGUI>();
            bless_tmp     = bless_obj.GetComponentInChildren<TextMeshProUGUI>();
            healBlock_tmp = healBlock_obj.GetComponentInChildren<TextMeshProUGUI>();
            immunity_tmp  = immunity_obj.GetComponentInChildren<TextMeshProUGUI>();
            silence_tmp   = silence_obj.GetComponentInChildren<TextMeshProUGUI>();
            curse_tmp     = curse_obj.GetComponentInChildren<TextMeshProUGUI>();
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