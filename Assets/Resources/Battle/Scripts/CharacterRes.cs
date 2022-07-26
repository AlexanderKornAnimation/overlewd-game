using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    [CreateAssetMenu(fileName = "New CharacterContent", menuName = "ScriptableObjects/CharacterContent")]
    public class CharacterRes : ScriptableObject
    {
        //public string charName;
        public string key;
        public Sprite icoPortrait;
        public Sprite bigPortrait;

        public float idleScale = 1f, battleScale = 1.4f;

        public Skill[] skill;
        public Skill[] pSkill;

        public GameObject characterPrefab = null;

        public string ani_idle_name = "idle";
        public string[] ani_pAttack_name = { "prepair1", "prepair2" };
        public string[] ani_attack_name = { "attack1", "attack2" };
        public string ani_defence_name = "defence";
        public string ani_defeat_name = "defeat";
    }
}