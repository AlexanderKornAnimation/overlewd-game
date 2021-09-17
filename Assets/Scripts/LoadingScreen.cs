using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public string loadingLabel { get; set; }

    private Texture2D screenTexture;

    void Awake()
    {
        screenTexture = Resources.Load<Texture2D>("Ulvi");
    }

    void Start()
    {
        
    }

    void OnGUI()
    {
        var rect = new Rect(0, 0, Screen.width, Screen.height);
        GUI.DrawTexture(rect, screenTexture);

        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.fontSize = (int)(Screen.height * 0.08);
        labelStyle.alignment = TextAnchor.MiddleCenter;
        labelStyle.normal.textColor = Color.black;
        int labelHeight = (int)(labelStyle.fontSize * 1.5);
        GUI.Label(new Rect(0, Screen.height - labelHeight, Screen.width, labelHeight), loadingLabel, labelStyle);
    }
}
