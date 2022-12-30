using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMemoryScreen
    {
        public abstract class BaseContent : MonoBehaviour
        {
            public int? memoryId { get; set; }
            public AdminBRO.MemoryItem memoryData => GameData.matriarchs.GetMemoryById(memoryId);
            
            private void Start()
            {
                Customize();
            }

            private void Customize()
            {                
                var pieces = transform.Find("Shards").GetComponentsInChildren<Piece>();
                
                foreach (var piece in pieces)
                {
                    piece.memoryId = memoryId;
                    piece.pieceData = memoryData?.pieces?.FirstOrDefault(p => p.shardName == piece.name);
                    piece.Customize();
                }
            }
        }
    }
}