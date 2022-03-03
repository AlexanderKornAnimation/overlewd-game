using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
	public class BattleManager : MonoBehaviour
	{
		public List<Character> characters;
		public List<CharController> characterControllerList;

		private int step = 0;

		//public List<CharacterController> conv;

		private void Start() => Initialize();
		

		private void Initialize()
		{
			characters = new List<Character>(Resources.LoadAll<Character>("BattlePersonages"));
			characters.Sort(SortByInitiative);
			foreach (var c in characters)
			{
				if (c.battleOrder > 0)
				{
					var charGO = new GameObject(c.name);
					var cc = charGO.AddComponent<CharController>();
					cc.character = c;
					characterControllerList.Add(cc);
				}
			}
			foreach (var c in characters)
			{
				Debug.Log($"Initiative Level {c.initiative}");
			}
			
			//characterControllerList.Sort();
		}

		private void Update()
		{
			switch (battleState)
			{
				case BattleState.PLAYERMOVE:
					//выбираем скилл и атакуем выбранного персонажа
					break;
				case BattleState.ENEMYMOVE:
					//получаем по зубам от случайного врага случайному персонажу
					break;
				case BattleState.INIT:

					break;
				case BattleState.WIN:
					break;
				case BattleState.LOSE:
					break;
			}


			if (Input.GetKeyDown(KeyCode.Z))
			{
				foreach (var cc in characterControllerList)
				{
					if (cc.isEnemy)
						cc.Defence();
					else
						cc.Attack(cc);
				}
			}

			if (Input.GetKeyDown(KeyCode.X))
			{
				foreach (var cc in characterControllerList)
				{
					cc.PlayIdle();
				}
			}
		}

		private void Step()
		{
			step++;
		}

		private int SortByInitiative(Character a, Character b)
		{
			if (a.initiative < b.initiative)
			{
				return 1;
			} else if (a.initiative > b.initiative)
			{
				return -1;
			} else if (a.isEnemy)
			{
				return -1;
			} else if (b.isEnemy)
			{
				return 1;
			}
			return 0;
		}

		public enum BattleState { PLAYERMOVE, ENEMYMOVE, INIT, WIN, LOSE }
		public BattleState battleState = BattleState.INIT;

	}
}
