using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using System;
using Spine;

namespace Overlewd
{
    public class SpineWidgetGroup : BaseWidget
    {
        public AdminBRO.Animation animationData { get; private set; }
        public List<SpineWidget> layers { get; private set; } = new List<SpineWidget>();

        public void Initialize(AdminBRO.Animation animationData)
        {
            this.animationData = animationData;

            foreach (var layerData in animationData.layouts)
            {
                var newLayer = SpineWidget.GetInstance(transform);
                newLayer.Initialize(layerData.animationPath, layerData.assetBundleId);
                newLayer.PlayAnimation(layerData.animationName, true);
                layers.Add(newLayer);
            }
        }

        public static SpineWidgetGroup GetInstance(Transform parent)
        {
            var groupGO = new GameObject(nameof(SpineWidgetGroup));
            var groupGO_rt = groupGO.AddComponent<RectTransform>();
            groupGO_rt.SetParent(parent, false);
            return groupGO.AddComponent<SpineWidgetGroup>();
        }
    }
}
