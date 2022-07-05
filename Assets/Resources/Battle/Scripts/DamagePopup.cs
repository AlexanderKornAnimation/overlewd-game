using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamagePopup : MonoBehaviour
{
    private TextMeshProUGUI text;
    private RectTransform rt;

    [SerializeField] private AnimationCurve curve;
    [SerializeField] private Color green, yellow, white, blue, red;

    private float lifetime = 1.5f;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        rt = GetComponent<RectTransform>();
    }
    private void Start()
    {
        
    }

    public void Setup(string msg, bool invertXScale, float delay = 0f, string textColor = null)
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
        if (invertXScale)
            rt.localScale = new Vector3(-1, 1, 1);
        text.SetText(msg);
        StartCoroutine(RunAnimation(delay));
    }
    IEnumerator RunAnimation(float startDelay)
    {
        yield return new WaitForSeconds(startDelay);
        rt.DOMoveY(800, 0.33f);
        rt.DOScaleY(0, lifetime).SetEase(curve);
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}