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
        public AdminBRO.Animation animationData { get; private set; }
        public List<SpineWidget> layers { get; private set; } = new List<SpineWidget>();

        private void Initialize(AdminBRO.Animation _animationData)
        {
            animationData = _animationData;
            foreach (var layerData in animationData.layouts)
            {
                var newLayer = SpineWidget.GetInstance(layerData, transform);
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

        public void TimeScale(float scale)
        {
            foreach (var layer in layers)
            {
                layer.TimeScale(scale);
            }
        }

        public static SpineScene GetInstance(AdminBRO.Animation animationData, Transform parent)
        {
            if (animationData == null)
                return null;

            var sceneGO = new GameObject(nameof(SpineScene));
            var sceneGO_rt = sceneGO.AddComponent<RectTransform>();
            sceneGO_rt.SetParent(parent, false);
            var ss = sceneGO.AddComponent<SpineScene>();
            ss.Initialize(animationData);
            return ss;
        }
    }
}