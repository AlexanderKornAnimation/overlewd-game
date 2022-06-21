using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private TextMeshProUGUI text;

    [SerializeField]
    private Color green, yellow, white, blue, red;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
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
        if (invertXScale) { 
            GetComponent<RectTransform>().localScale = new Vector3 (-1,1,1);
        }

        text.SetText(msg);

        

        Destroy(gameObject, 1.45f + delay);
    }
}