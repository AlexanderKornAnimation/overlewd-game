using System;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMemoryScreen
    {
        public class Piece : MonoBehaviour
        {
            private Image image;
            private Button button;

            public AdminBRO.MemoryItem.Piece pieceData;
            public int? memoryId { get; set; }

            private void Awake()
            {
                image = GetComponent<Image>();
                image.alphaHitTestMinimumThreshold = 1f;

                button = GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
            }

            public void Customize()
            {
                ColorUtility.TryParseHtmlString("#50d749", out var colorAdvanced);
                ColorUtility.TryParseHtmlString("#9c5fee", out var colorEpic);
                ColorUtility.TryParseHtmlString("#ffb526", out var colorHeroic);

                gameObject.SetActive(!pieceData.isPurchased);

                image.color = pieceData?.rarity switch
                {
                    AdminBRO.Rarity.Advanced => colorAdvanced,
                    AdminBRO.Rarity.Heroic => colorHeroic,
                    AdminBRO.Rarity.Epic => colorEpic,
                    _ => Color.white
                };
            }

            private async void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                var shardData = GameData.matriarchs.GetShardById(pieceData?.shardId, pieceData?.rarity);

                if (shardData.amount > 0)
                {
                    await GameData.matriarchs.MemoryPieceOfGlassBuy(memoryId, pieceData?.shardName);
                    gameObject.SetActive(false);
                }
            }
        }
    }
}