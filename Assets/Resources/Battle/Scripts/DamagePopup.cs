using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    private TextMeshProUGUI text;
    private RectTransform rt;

    [SerializeField] private AnimationCurve curve;
    [SerializeField] private Color green, yellow, white, blue, red;

    public float lifetime = 1.5f;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        rt = GetComponent<RectTransform>();
    }

    public void Setup(string msg, bool invertXScale, float delay = 0f, string textColor = null, int yOffset = 0)
    {
        switch (textColor)
        {
            case "green":
                text.color = green;
                break;
            case "yellow":
                text.color = yellow;
                break;
            case "white":
                text.color = white;
                break;
            case "blue":
                text.color = blue;
                break;
            default:
                text.color = red;
                break;
        }
        text.alpha = 0f;
        if (invertXScale)
            rt.localScale = new Vector3(-1, 1, 1);
        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, rt.anchoredPosition.y + yOffset);
        text.SetText(msg);
        StartCoroutine(RunAnimation(delay, yOffset));
    }
    IEnumerator RunAnimation(float startDelay, int yOffset)
    {
        yield return new WaitForSeconds(startDelay);
        text.alpha = 1f;
        rt.DOMoveY(800 + yOffset, 0.33f);
        rt.DOScaleY(0, lifetime).SetEase(curve);
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}