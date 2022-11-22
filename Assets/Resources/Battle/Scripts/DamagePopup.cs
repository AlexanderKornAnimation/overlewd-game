using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    private TextMeshProUGUI text;
    private RectTransform rt;
    private RectTransform backRt;

    private string color = "",
        white = "<color=#FFFFFF>",
        red = "<color=#FD4D4B>",
        blue = "<color=#5C9BCC>",
        yellow = "<color=#FFBA53>",
        green = "<color=#83FAB4>",
        purple = "<color=#7D35FF>";
    private Animator ani;

    public float lifetime = .75f;

    private void Awake()
    {
        text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        rt = GetComponent<RectTransform>();
        ani = GetComponent<Animator>();
        backRt = transform.Find("Back")?.GetComponent<RectTransform>();
    }

    public void Setup(string msg, bool invertXScale, float delay = 0f,
        string textColor = null, int yOffset = 0, float scale = 1f, bool fast = false, bool boss = false)
    {
        if (text)
        {
            color = textColor switch
            {
                "green" => green,
                "red" => red,
                "blue" => blue,
                "yellow" => yellow,
                "purple" => purple,
                _ => white
            };
            text.SetText(color + msg);
        }
        if (invertXScale)
            rt.localScale = new Vector3(-1, 1, 1) * scale;
        if (boss && backRt != null)
            backRt.localScale = new Vector3(-1,1,1) * scale;
        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, rt.anchoredPosition.y + yOffset);
        StartCoroutine(RunAnimation(delay, yOffset, fast));
    }
    IEnumerator RunAnimation(float startDelay, int yOffset, bool fast)
    {
        yield return new WaitForSeconds(startDelay);
        if (fast)
            ani?.Play("Base Layer.Fast");
        else {
            ani?.Play("Base Layer.Long");
            lifetime += 1f;
        }
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}