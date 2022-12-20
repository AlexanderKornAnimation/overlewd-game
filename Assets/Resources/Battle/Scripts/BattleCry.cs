using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class BattleCry : MonoBehaviour
    {
        public enum CryEvent
        {
            BossOneShot = 0,
            OneSpellKill = 1,
            OneShoot = 2,
            HalfKill = 3,
            MaxHP = 4,
        }
        List<CryEvent> allEvents = new List<CryEvent>(5);
        private string[] oneSpellKill = new string[] { "Flawless victory!", "Toasty!", "You’re done!" };
        private string[] halfKill = new string[] { "Brutal!", "That’s a lot of damage!", "Surrender now!" };
        private string[] bossOneShot = new string[] { "Witness me!", "Get over here!", "Exterminate!" };
        private string[] maxHP = new string[] { "Stayin' alive!", "Not on my watch!", "You’re fine!", "Stayin' alive!", "Not on my watch!", "You’re fine!" };
        private string[] oneShoot = new string[] { "One shot!", "And stay down!", "Die!" };

        GameObject cryPrefab;
        bool isBoss = false;
        bool invertX = false;
        int yOffset = 0;
        int killCount = 0;

        private void Awake()
        {
            cryPrefab = Resources.Load("Battle/Prefabs/BattleCry") as GameObject;
        }

        public void SetUp(bool invertX, bool isBoss, int yOffset)
        {
            this.invertX = invertX;
            this.isBoss = isBoss;
            this.yOffset = yOffset;
        }

        public void CallBattleCry(CryEvent cry)
        {
            allEvents.Add(cry);
        }

        public void AddKill(int killsNeeded, bool boss)
        {
            killCount++;
            if (killCount >= killsNeeded)
            {
                if (boss)
                    CallBattleCry(CryEvent.BossOneShot);
                else
                    CallBattleCry(CryEvent.OneSpellKill);
            }
        }

        public void ShowBattleCry()
        {
            if (allEvents.Count > 0)
                StartCoroutine(SortAndSelect());
            killCount = 0;
        }

        IEnumerator SortAndSelect()
        {
            yield return new WaitForSeconds(0.1f);
            if (allEvents.Count > 0)
            {
                //allEvents = allEvents.Distinct().ToList();
                allEvents.Sort();

                var randomPhrase = Random.Range(0, 3);
                switch (allEvents[0])
                {
                    case CryEvent.OneSpellKill:
                        CreateBattleCryPopUp(oneSpellKill[randomPhrase]);
                        Debug.Log($"{gameObject.name}: all enemies are defeated with one spell");
                        break;
                    case CryEvent.HalfKill:
                        CreateBattleCryPopUp(halfKill[randomPhrase]);
                        Debug.Log($"{gameObject.name}: one hit takes more than 50% of enemy health");
                        break;
                    case CryEvent.BossOneShot:
                        CreateBattleCryPopUp(bossOneShot[randomPhrase]);
                        Debug.Log($"{gameObject.name}: as boss kills Overlord’s team in one shot (not Overlord)");
                        break;
                    case CryEvent.MaxHP:
                        CreateBattleCryPopUp(maxHP[randomPhrase]);
                        Debug.Log($"{gameObject.name}: as healer heals one teammate to full health");
                        break;
                    case CryEvent.OneShoot:
                        CreateBattleCryPopUp(oneShoot[randomPhrase]);
                        Debug.Log($"{gameObject.name}: one hit kill of a single character");
                        break;
                }
                allEvents.Clear();
            }
        }

        private void CreateBattleCryPopUp(string msg)
        {
            Instantiate(cryPrefab, transform).GetComponent<DamagePopup>().Setup(msg, invertX, yOffset: yOffset, boss: isBoss);
        }
    }
}