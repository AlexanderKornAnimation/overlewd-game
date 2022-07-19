using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleLog : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI log;
    string logText;

    static BattleLog bl;
    private void Awake() => bl = this;

    
    public void Add(string text)
    {
        if (log) log.text += (text + "\n");
    }
}
