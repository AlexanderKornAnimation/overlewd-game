using UnityEngine;

namespace Overlewd
{
    [CreateAssetMenu(fileName = "New CharacterContent", menuName = "ScriptableObjects/CharacterContent")]
    public class CharacterRes : ScriptableObject
    {
        //public string charName;
        public string key;
        public Sprite ico;
        public Sprite battlePortrait;
        public Sprite bigPortrait;

        public float idleScale = 1f, battleScale = 1f;
        public float yOffset = 0f;

        public Skill[] skill;

        public string
            idle_Prefab_Path = "Battle/BattlePersonages/idle_SkeletonData";
        public GameObject characterPrefab = null;
        public string
            ani_idle_name = "idle",
            ani_pAttack_1_name = "prepair1",
            ani_pAttack_2_name = "prepair2",
            ani_attack_1_name = "attack1",
            ani_attack_2_name = "attack2",
            ani_defence_name = "defence",
            ani_defeat_name = "defeat";
            
    }
}