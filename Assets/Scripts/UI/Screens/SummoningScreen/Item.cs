using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSSummoningScreen
    {
        public abstract class Item : MonoBehaviour
        {
            protected Image shardBackground;
            protected Image girl;
            protected RectTransform rect;
            protected SpineWidget anim;
            public Vector2 endPos { get; set; }
            public string tabType { get; set; }

            protected virtual void Awake()
            {
                anim = gameObject.AddComponent<SpineWidget>();
                rect = GetComponent<RectTransform>();
            }

            protected virtual async Task PlayAnimation()
            {
                anim?.PlayAnimation("cr_gold", true);

                await Task.CompletedTask;
            }

            public async Task Show()
            {
                float duration = 0.2f;

                await rect.DOMove(endPos, duration).
                    SetEase(Ease.OutExpo)
                    .AsyncWaitForCompletion();

                await PlayAnimation();
            }
        }
    }
}