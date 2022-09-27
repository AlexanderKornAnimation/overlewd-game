using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    private TextMeshProUGUI text;
    private RectTransform rt;

    [SerializeField] private AnimationCurve curve;
    [SerializeField] private Color green, yellow, white, blue, red, purple;
    private Animator ani;

    public float lifetime = .75f;

    private void Awake()
    {
        text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        rt = GetComponent<RectTransform>();
        ani = GetComponent<Animator>();
    }

    public void Setup(string msg, bool invertXScale, float delay = 0f, string textColor = null, int yOffset = 0)
    {
        if (text)
        {
            text.color = textColor switch
            {
                "green" => green,
                "yellow" => yellow,
                "white" => white,
                "blue" => blue,
                "purple" => purple,
                _ => red
            };
            text.SetText(msg);
        }
        if (invertXScale)
            rt.localScale = new Vector3(-1, 1, 1);
        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, rt.anchoredPosition.y + yOffset);
        StartCoroutine(RunAnimation(delay, yOffset));
    }
    IEnumerator RunAnimation(float startDelay, int yOffset)
    {
        yield return new WaitForSeconds(startDelay);
        ani?.Play("Base Layer.BattlePopupAnimation");
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}