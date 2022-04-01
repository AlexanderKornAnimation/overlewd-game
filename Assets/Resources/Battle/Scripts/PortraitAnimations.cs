using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PortraitAnimations : MonoBehaviour
{
	private float prevtX, x;
	private RectTransform rt;
	private bool move = false;
	private float moveTime = .1f;

	private void Start() => x = rt.position.x;

	private void Update()
	{
		if (x != prevtX && !move)
		{
			move = true;
			rt.DOAnchorPosX(x, moveTime);
			StartCoroutine(TurnOn());
		}
	}

	private void OnDisable()
	{
		StartCoroutine(TurnOn());
	}
	IEnumerator TurnOn()
	{
		yield return new WaitForSeconds(.1f);
		gameObject.SetActive(true);
	}
	IEnumerator Wait()
	{
		yield return new WaitForSeconds(moveTime + .01f);
		move = false;
	}
}
