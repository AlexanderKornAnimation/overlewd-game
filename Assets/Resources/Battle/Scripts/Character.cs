using UnityEngine;

namespace Overlewd
{
    [CreateAssetMenu(fileName = "New Character", menuName = "ScriptableObjects/Character")]
    public class Character : ScriptableObject
    {
        [SerializeField] private int order = -1; //3,2,1 = on the table; -1 = in the deck;
        public int Order { get => order; set => order = value; }
        public string charName;
        public Sprite ico;
        public Sprite battlePortrait;
        public Sprite bigPortrait;

        public bool isEnemy = false;
        public bool isBoss = false;
        public bool isOverlord = false;

        private enum CharClass { ASSASIN, TANK, BRUISER, SUPPORT, CASTER }
        [SerializeField] private CharClass charClass;

        public float idleScale = 1f, battleScale = 1f;
        public float yOffset = 0f;

        public int
            speed = 10,
            power = 10,
            constitution = 10,
            agility = 10;

        public float
            accuracity = 10,    //+1% from agility
            dodge = 10,         //+1% from agility
            critRate = 10,      //1% from speed
            damage = 10;        //+8 from power
        public int
            maxHp = 250,        //25 from const;
            maxMp = 125;        //HP/2;

        //default scales change from character class
        public float
            accuracityScale = 10,
            dodgeScale = 1,
            damageScale = 8,
            critRateScale = 1,
            hpScale = 25;

        public Skill[] skill;
        public Item itemSlot1 = null, itemSlot2 = null;

        public int level = 1, maxLevel = 10;
        public float levelStatScale = 1.1f;
        public int xp = 0, xpToNextlUp = 1000;
        public int hp = 25;
        public int mp = 100;

        public bool isDead = false;

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

        public void ApplyStats()
        {
            switch (charClass)
            {
                case CharClass.ASSASIN:
                    accuracityScale = 10;
                    dodgeScale = 1;
                    damageScale = 8;
                    critRateScale = 1;
                    hpScale = 25;
                    break;
                case CharClass.BRUISER:
                    accuracityScale = 10;
                    dodgeScale = 1;
                    damageScale = 8;
                    critRateScale = 1;
                    hpScale = 25;
                    break;
                case CharClass.TANK:
                    accuracityScale = 10;
                    dodgeScale = 1;
                    damageScale = 8;
                    critRateScale = 1;
                    hpScale = 25;
                    break;
                case CharClass.SUPPORT:
                    accuracityScale = 10;
                    dodgeScale = 1;
                    damageScale = 8;
                    critRateScale = 1;
                    hpScale = 25;
                    break;
                case CharClass.CASTER:
                    accuracityScale = 10;
                    dodgeScale = 1;
                    damageScale = 8;
                    critRateScale = 1;
                    hpScale = 25;
                    break;
            }
            var scale = level * levelStatScale;
            accuracity = 60 + Mathf.RoundToInt(agility * scale * damageScale); //Base 60 + agi scales
            critRate = Mathf.RoundToInt(agility * scale * damageScale);
            dodge = Mathf.RoundToInt(agility * scale * damageScale);
            damage = Mathf.RoundToInt(power * scale * damageScale);

            maxHp = Mathf.RoundToInt(constitution * levelStatScale * hpScale);
            maxMp = maxHp / 2;
            hp = maxHp;
            mp = maxMp;

            if (itemSlot1 != null)
            {
                damage += itemSlot1.attack;
                constitution += itemSlot1.defence;
            }
            if (itemSlot2 != null)
            {
                damage += itemSlot2.attack;
                constitution += itemSlot2.defence;
            }
        }

        public void Save(int saveHP, int saveMP)
        {
            hp = saveHP;
            mp = saveMP;
        }


    }
}