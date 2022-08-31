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
                    //all shards is opened
                }
            }

        }
    }
}