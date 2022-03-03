using UnityEngine;

namespace Overlewd
{
	[CreateAssetMenu(fileName = "New Character", menuName = "ScriptableObjects/Character")]
	public class Character : ScriptableObject
	{
		public string charName;
		public Sprite ico;

		public int battleOrder = -1; //3,2,1 = on the table; -1 = in the deck;

		public bool isEnemy = false;
		public bool isBoss = false;

		private enum CharClass { ASSASIN, TANK, WARRIOR, SUPPORT }
		[SerializeField] private CharClass charClass;

		public float idleScale = 0.5f, battleScale = 0.7f;
		public float yOffset = 0f;

		public int initiative = 5, attack = 10, defence = 5, agility = 5;
		public Item itemSlot1 = null, itemSlot2 = null;

		public int level = 1, maxLevel = 10;
		public int xp = 0, xpToNextlUp = 1000;
		public int hp = 25, maxHp = 25;
		public int mp = 100, maxMp = 100;

		public bool isDead = false;

		public string folder = "";

		public string
			ani_idle_path = "idle0_SkeletonData",
			ani_pAttack_1_path = "prepair1_SkeletonData",
			ani_pAttack_2_path = "prepair2_SkeletonData",
			ani_attack_1_path = "attack1_SkeletonData",
			ani_attack_2_path = "attack2_SkeletonData",
			ani_defence_path = "defence_SkeletonData",
			ani_difeat_path = "difeat_SkeletonData";
		public string
			ani_idle_name = "idle",
			ani_pAttack_1_name = "prepair1",
			ani_pAttack_2_name = "prepair2",
			ani_attack_1_name = "attack1",
			ani_attack_2_name = "attack2",
			ani_defence_name = "defence",
			ani_difeat_name = "difeat";

		public void ApplyBonus()
		{
			switch (charClass)
			{
				case CharClass.ASSASIN:
					agility += Mathf.RoundToInt(level * 2);
					break;
				case CharClass.WARRIOR:
					attack += Mathf.RoundToInt(level * 2);
					break;
				case CharClass.TANK:
					defence += Mathf.RoundToInt(level * 2);
					break;
				case CharClass.SUPPORT:
					hp += Mathf.RoundToInt(level * 5);
					defence += Mathf.RoundToInt(level * 1.5f);
					break;
			}

			if (itemSlot1 != null)
			{
				attack += itemSlot1.attack;
				defence += itemSlot1.defence;
			}
			if (itemSlot2 != null)
			{
				attack += itemSlot2.attack;
				defence += itemSlot2.defence;
			}
		}

		public void Save(int saveHP, int saveMP)
		{
			hp = saveHP;
			mp = saveMP;
		}


	}
}