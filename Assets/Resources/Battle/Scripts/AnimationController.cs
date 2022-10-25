using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(Animator))]

public class AnimationController : MonoBehaviour
{
    /// <summary>
    /// Script clear the scene from the assigned animation
    /// </summary>

    Animator ani;
    [SerializeField] float duration;
    [SerializeField] TextMeshProUGUI tmp;

    private void Awake()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        ani = GetComponent<Animator>();
    }
    void Start()
    {
        duration = ani.runtimeAnimatorController.animationClips[0].averageDuration;
        //Debug.Log($"Animation duration {duration}");
        Destroy(this.gameObject, duration);
    }
    public void SetUp(string txt)
    {
        if (tmp) tmp.text = txt;
    }

    
}
