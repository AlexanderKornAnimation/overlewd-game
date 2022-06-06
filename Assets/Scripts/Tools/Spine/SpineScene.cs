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
        public AdminBRO.AnimationScene sceneData { get; private set; }
        public List<SpineWidget> layers { get; private set; } = new List<SpineWidget>();

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

        public static SpineScene GetInstance(AdminBRO.AnimationScene sceneData, Transform parent)
        {
            var sceneGO = new GameObject(nameof(SpineScene));
            var sceneGO_rt = sceneGO.AddComponent<RectTransform>();
            sceneGO_rt.SetParent(parent, false);

            var spineScene = sceneGO.AddComponent<SpineScene>();
            spineScene.sceneData = sceneData;
            foreach (var layerData in sceneData.layouts)
            {
                var newLayer = SpineWidget.GetInstance(layerData.animationPath,
                    layerData.assetBundleId, sceneGO_rt);
                newLayer?.PlayAnimation(layerData.animationName, true);
                if (newLayer != null) spineScene.layers.Add(newLayer);
            }

            return spineScene;
        }
    }
}