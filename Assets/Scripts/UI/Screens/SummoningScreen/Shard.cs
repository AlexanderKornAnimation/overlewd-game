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
        public class Shard : MonoBehaviour
        {
            private Image shardBackground;
            private Image girl;
            private RectTransform rect;
            private SpineWidget anim;
            public Vector2 endPos { get; set; }
            public string tabType { get; set; }

            private void Awake()
            {
                rect = GetComponent<RectTransform>();
            }

            private async Task PlayAnimation()
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
            
            private void Start()
            {
                Customize();
            }

            private void Customize()
            {
                anim = tabType switch
                {
                    AdminBRO.GachItem.TabType_Matriachs => SpineWidget.GetInstance(
                        "Prefabs/UI/Screens/SummoningScreen/Animations/BattleGirls/BattleGirlsAnim", transform),
                    AdminBRO.GachItem.TabType_Shards => SpineWidget.GetInstance(
                        "Prefabs/UI/Screens/SummoningScreen/Animations/Shards/ShardAnim", transform),
                    AdminBRO.GachItem.TabType_CharactersEquipment => SpineWidget.GetInstance(
                        "Prefabs/UI/Screens/SummoningScreen/Animations/Equip/EquipAnim", transform),
                    AdminBRO.GachItem.TabType_OverlordEquipment => SpineWidget.GetInstance(
                        "Prefabs/UI/Screens/SummoningScreen/Animations/Equip/EquipAnim", transform),
                    _ => SpineWidget.GetInstance(
                        "Prefabs/UI/Screens/SummoningScreen/Animations/BattleGirls/BattleGirlsAnim", transform),
                };
            }

            public static Shard GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Shard>("Prefabs/UI/Screens/SummoningScreen/Item",
                    parent);
            }
        }
    }
}