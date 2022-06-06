using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Overlewd
{
    namespace NSSummoningScreen
    {
        public abstract class Item : MonoBehaviour
        {
            protected Image itemImage;
            protected RectTransform rect;
            protected SpineWidget anim;
            protected Image mask;
            public Vector2 endPos { get; set; }
            public string tabType { get; set; }

            protected virtual void Awake()
            {
                anim = gameObject.AddComponent<SpineWidget>();
                anim.Initialize();
                rect = GetComponent<RectTransform>();
                mask = transform.Find("Mask").GetComponent<Image>();
                itemImage = transform.Find("Mask").Find("Item").GetComponent<Image>();
                itemImage.gameObject.SetActive(false);
            }

            public async Task ShowAsync()
            {
                float duration = 0.2f;

                await rect.DOMoveY(endPos.y, duration).
                    SetEase(Ease.OutExpo)
                    .AsyncWaitForCompletion();

                var landingParticle = ResourceManager.InstantiateWidgetPrefab(
                    "Prefabs/UI/Screens/SummoningScreen/Animations/FX/land.gold.part", transform);

                anim.PlayAnimation("cr_gold", true);

                await Task.CompletedTask;
            }

            public async Task OpenAsync()
            {
                var openShardParticle = ResourceManager.InstantiateWidgetPrefab(
                    "Prefabs/UI/Screens/SummoningScreen/Animations/FX/openShard.part",transform);
                
                await Task.Delay(100);
                
                itemImage.gameObject.SetActive(true);
                
                await Task.CompletedTask;
            }
        }
    }
}