using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public abstract class BaseBuilding : MonoBehaviour
        {
            public int? buildingId { get; set; }
            protected AdminBRO.Building buildingData => GameData.buildings.GetBuildingById(buildingId);
            protected List<GameObject> levels = new List<GameObject>();

            protected virtual void Start()
            {
                Customize();
            }

            protected virtual void Customize()
            {
                if (buildingData != null)
                {
                    for (int i = 1; i <= buildingData.levels.Count; i++)
                    {
                        levels.Add(transform.Find($"Level{i}").gameObject);
                    }
                
                    if (buildingData.currentLevel.HasValue)
                    {
                        for (int i = 0; i < buildingData.levels.Count; i++)
                        {
                            levels[i].SetActive(buildingData.currentLevel == i);
                        }
                    }
                }
            }
            
            public async Task ShowAsync()
            {
                await UITools.FadeShowAsync(gameObject, 0.7f);
            }

            public void Hide()
            {
                UITools.FadeHide(gameObject);
            }
        }
    }
}