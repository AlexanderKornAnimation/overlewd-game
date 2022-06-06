using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using System;
using System.Linq;
using Spine;

namespace Overlewd
{
    public class SpineScene : BaseWidget
    {
        public AdminBRO.AnimationScene animationData { get; private set; }
        public List<SpineWidget> layers { get; private set; } = new List<SpineWidget>();

        public void Initialize(AdminBRO.AnimationScene animationData)
        {
            this.animationData = animationData;

            foreach (var layerData in animationData.layouts)
            {
                var newLayer = SpineWidget.GetInstance(layerData.animationPath,
                    layerData.assetBundleId, transform);
                newLayer?.PlayAnimation(layerData.animationName, true);
                if (newLayer != null) layers.Add(newLayer);
            }
        }

        public void Play()
        {
            foreach (var layer in layers)
            {
                layer.Play();
            }
        }

        public void Pause()
        {
            foreach (var layer in layers)
            {
                layer.Pause();
            }
        }

        public static SpineScene GetInstance(Transform parent)
        {
            var groupGO = new GameObject(nameof(SpineScene));
            var groupGO_rt = groupGO.AddComponent<RectTransform>();
            groupGO_rt.SetParent(parent, false);
            return groupGO.AddComponent<SpineScene>();
        }
    }
}