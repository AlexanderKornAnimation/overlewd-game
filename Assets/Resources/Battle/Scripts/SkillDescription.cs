using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class SkillDescription : MonoBehaviour
    {
        [HideInInspector]
        public SkillController sc;
        Button btn;
        Animator ani;
        Image skillIco;
        Image effectSlot;
        TextMeshProUGUI Name, description, coolDown, level;
        GameObject counterTurnsGO;
        [HideInInspector]
        public bool isOpen = false;

        private void Awake()
        {
            btn = GetComponent<Button>();
            ani = GetComponent<Animator>();
            Name = transform.Find("Name").GetComponent<TextMeshProUGUI>();
            description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
            skillIco = transform.Find("Skill").GetComponent<Image>();
            effectSlot = transform.Find("Skill/status").GetComponent<Image>();
            counterTurnsGO = transform.Find("CounterTurns").gameObject;
            coolDown = counterTurnsGO.transform.Find("Counter").GetComponent<TextMeshProUGUI>();
            level = transform.Find("Skill/level/text").GetComponent<TextMeshProUGUI>();
            btn.onClick.AddListener(Close);
        }
        public void Open(SkillController skillController)
        {
            sc = skillController;
            effectSlot.gameObject.SetActive(sc.effectSlot.gameObject.activeSelf);
            effectSlot.sprite = sc.effectSlot.sprite;
            skillIco.sprite = sc.image.sprite;
            Name.text = sc.Name;
            level.text = sc.level.ToString();
            description.text = sc.description;
            counterTurnsGO.SetActive(sc.cooldown > 0);
            coolDown.text = $"{sc.cooldown} turns";
            if (!isOpen)
                ani.SetTrigger("Open");
            isOpen = true;
        }
        public void Close()
        {
            if (isOpen)
            {
                ani.SetTrigger("Close");
                isOpen = false;
            }
        }
    }
}