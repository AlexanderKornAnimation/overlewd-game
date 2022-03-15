using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
	public class BattleManager : MonoBehaviour
	{
		[HideInInspector] public List<Character> characters;
		[HideInInspector] public List<CharController> charControllerList;
		[HideInInspector] public List<CharController> targetsForEnemy;
		[HideInInspector] public List<GameObject> QueueElements;
		[SerializeField] private float portraitScale = 1.5f;
		public CharController onClick;
		public Animator ani;
		public GameObject portraitPrefab; //attach to BM in inspector
		public Transform QueueUIContent;
		public Color redColor;
		public Button attack_1, attack_2, attack_3;


		private int step = 0; //current characters list step
		private int maxStep = 2; //max count of battle queue, take from character's list
		private int wave = 1, maxWave = 3;

		private void Start() => Initialize();

		private void Initialize()
		{
			attack_1.onClick.AddListener(Button1);
			attack_2.onClick.AddListener(Button2);
			//attack_3.onClick.AddListener(EnemyAttack1);

			if (QueueUIContent == null)
				QueueUIContent = transform.Find("BattleUICanvas/QueueUI/Content");
			if (portraitPrefab == null)
				portraitPrefab = Resources.Load("Prefabs/Battle/Portrait") as GameObject;
			characters = new List<Character>(Resources.LoadAll<Character>("BattlePersonages"));
			characters.Sort(SortByInitiative);
			foreach (var c in characters)
			{
				if (c.battleOrder > 0)
				{
					var charGO = new GameObject(c.name);
					var cc = charGO.AddComponent<CharController>();
					cc.character = c;
					charControllerList.Add(cc);
					cc.bm = this;
					if (!c.isEnemy)
						targetsForEnemy.Add(cc);
					//Fill QueueUI characters icons
					var portraitQ = Instantiate(portraitPrefab, QueueUIContent);
					portraitQ.name = "Portrait_" + c.name;
					portraitQ.GetComponent<Image>().sprite = c.ico;
					portraitQ.GetComponent<Button>().onClick.AddListener(cc.Select);
					if (c.isEnemy) portraitQ.transform.Find("color").GetComponent<Image>().color = redColor; //Switch color on portrait indicator from blue to red
					QueueElements.Add(portraitQ);
				}
			}
			maxStep = charControllerList.Count - 1;
			QueueElements[step].transform.localScale *= portraitScale; //Scale Up First Element
			if (charControllerList[0].isEnemy)
				battleState = BattleState.ENEMY;
			else
				battleState = BattleState.PLAYER;
			StartCoroutine(LateInit());
		}

		IEnumerator LateInit()
		{
			yield return new WaitForSeconds(0.01f);
			charControllerList[step].Select();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Z))
			{
				Button1();
			}
			if (Input.GetKeyDown(KeyCode.X))
			{
				Button2();
			}
			if (Input.GetKeyDown(KeyCode.C))
			{
				//EnemyAttack1();
			}
		}

		public void Button1()
		{
			if (onClick != null && battleState == BattleState.PLAYER) //|| onClick.isEnemy == false
			{
				ani.SetTrigger("Player");
				charControllerList[step].Attack(0, onClick);
				onClick.Defence();
				battleState = BattleState.ANIMATION;
			}
		}
		public void Button2()
		{
			if (onClick != null && battleState == BattleState.PLAYER)
			{
				ani.SetTrigger("Player");
				charControllerList[step].Attack(1, onClick);
				onClick.Defence();
				battleState = BattleState.ANIMATION;
			}
		}

		IEnumerator EnemyAttack1()
		{
			yield return new WaitForSeconds(0.5f);
			onClick = targetsForEnemy[Random.Range(0, targetsForEnemy.Count)];
			yield return new WaitForSeconds(0.5f);
			if (onClick != null && battleState == BattleState.ENEMY)
			{
				ani.SetTrigger("Enemy");
				charControllerList[step].Attack(Random.Range(0, 2), onClick);
				onClick.Defence();
				battleState = BattleState.ANIMATION;
			}
		}

		public void BattleOut()
		{
			ani.SetTrigger("BattleOut");
			Step();
			charControllerList[step].Highlight();
			onClick = null;
		}

		private void Step()
		{
			var qe = QueueElements[step];
			qe.transform.SetSiblingIndex(maxStep);
			qe.transform.localScale = Vector3.one;
			if (step < maxStep) step++; else step = 0;
			qe.transform.SetSiblingIndex(maxStep); //Move first element to back after Step++
			QueueElements[step].transform.localScale *= portraitScale;
			if (charControllerList[step].isEnemy)
			{
				battleState = BattleState.ENEMY;
				StartCoroutine(EnemyAttack1());
			} else
				battleState = BattleState.PLAYER;
		}

		private int SortByInitiative(Character a, Character b)
		{
			if (a.initiative < b.initiative)
				return 1;
			else if (a.initiative > b.initiative)
				return -1;
			else if (a.isEnemy)
				return -1;
			else if (b.isEnemy)
				return 1;
			return 0;
		}

		public enum BattleState { PLAYER, ENEMY, ANIMATION, INIT, WIN, LOSE }
		public BattleState battleState = BattleState.INIT;
	}
}
