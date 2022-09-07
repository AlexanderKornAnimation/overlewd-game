using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSSummoningScreen
    {
        public class DropEvent : MonoBehaviour
        {
            Animator ani;
            public List<ItemDropController> items;
            public List<Material> mat;
            public List<GameObject> landParticles = null;
            public int opened = 0;
            public bool allShardsOpened = false;

            private void Awake()
            {
                ani = GetComponent<Animator>();
            }

            public void DisableAnimator()
            {
                if (ani) ani.enabled = false;
                if (items != null)
                    foreach (var i in items)
                    {
                        i.canClick = true;
                        i.parentDE = this;
                    }
            }

            public void ShardIsOpen()
            {
                opened++;
                if (opened >= items.Count)
                {
                    allShardsOpened = true;
                }
            }

            public bool IsComplete => allShardsOpened;

            public void SetShardsData(SummoningScreenShardsData shardsData)
            {
                int shape = 0;
                if (shardsData.isMemoriesType) shape = 0;
                if (shardsData.isEquipmentsType) shape = 1;
                if (shardsData.isBattleCharactersType) shape = 2;

                for (int i = 0; i < items.Count; i++)
                {
                    int maxGrade = 0;
                    if (shardsData.shards[i].isAdvanced) maxGrade = 1;
                    if (shardsData.shards[i].isEpic) maxGrade = 2;
                    if (shardsData.shards[i].isHeroic) maxGrade = 3;
                    items[i].SetUp(shape, maxGrade, shardsData.shards[i].icon);
                }
            }
        }
    }
}