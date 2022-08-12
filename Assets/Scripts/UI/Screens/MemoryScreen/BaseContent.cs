using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMemoryScreen
    {
        public abstract class BaseContent : MonoBehaviour
        {
            protected Image[] shardsImages = new Image[10];
            protected Button[] shardButtons = new Button[10]; 
            protected Image background;
            protected Color colorAdvanced;
            protected Color colorEpic;
            protected Color colorHeroic;

            protected virtual void Awake()
            {
                var shards = transform.Find("Shards");

                background = transform.Find("Background").GetComponent<Image>();

                for (int i = 0; i < shardsImages.Length; i++)
                {
                    var shard = shards.Find($"Shard{i + 1}");
                    shardButtons[i] = shard.GetComponent<Button>();
                    shardsImages[i] = shard.GetComponent<Image>();
                }
                
                ColorUtility.TryParseHtmlString("#50d749", out colorAdvanced);
                ColorUtility.TryParseHtmlString("#9c5fee", out colorEpic);
                ColorUtility.TryParseHtmlString("#ffb526", out colorHeroic);
            }
            
            private void Start()
            {
                Customize();
            }

            protected virtual void Customize()
            {
                shardsImages[0].color = colorAdvanced;
                shardsImages[1].color = colorAdvanced;
                shardsImages[2].color = colorAdvanced;
                shardsImages[3].color = colorEpic;
                shardsImages[4].color = colorEpic;
                shardsImages[5].color = colorHeroic;
                shardsImages[6].color = colorHeroic;
                shardsImages[7].color = colorHeroic;
                shardsImages[8].color = colorHeroic;
                shardsImages[9].color = colorHeroic;
            }
        }
    }
}