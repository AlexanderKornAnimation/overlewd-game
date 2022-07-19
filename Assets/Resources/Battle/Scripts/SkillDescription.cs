using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
        TextMeshProUGUI Name, description;
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
            btn.onClick.AddListener(Close);
        }
        public void Open(SkillController skillController)
        {
            sc = skillController;
            effectSlot.gameObject.SetActive(sc.effectSlot.gameObject.activeSelf);
            effectSlot.sprite = sc.effectSlot.sprite;
            skillIco.sprite = sc.image.sprite;
            Name.text = sc.Name;
            description.text = sc.description;
            if (!isOpen)
                ani.SetTrigger("Open");

            isOpen = true;
            //StartCoroutine(AutoClose());
        }
        public void Close()
        {
            isOpen = false;
            ani.SetTrigger("Close");
        }
        IEnumerator AutoClose()
        {
            yield return new WaitForSeconds(25f);
            Close();
        }
    }
}