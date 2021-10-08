using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class SpineWidget : BaseWidget
    {
        public string skeletonDataPath { get; set; }
        public string animName { get; set; }
        public bool multipleRenderCanvas { get; set; }

        void Awake()
        {
            
        }

        void Start()
        {
            var skeletonDataAsset = Resources.Load<Spine.Unity.SkeletonDataAsset>(skeletonDataPath);
            var spineGO_sg = gameObject.AddComponent<Spine.Unity.SkeletonGraphic>();
            spineGO_sg.allowMultipleCanvasRenderers = multipleRenderCanvas;
            spineGO_sg.skeletonDataAsset = skeletonDataAsset;
            spineGO_sg.Initialize(false);
            spineGO_sg.AnimationState.SetAnimation(0, animName, true);
        }

        void Update()
        {

        }

        public static SpineWidget Attacht(string name, Transform parent)
        {
            var spineGO = new GameObject(name);
            var spineGO_rt = spineGO.AddComponent<RectTransform>();
            spineGO_rt.SetParent(parent, false);
            return spineGO.AddComponent<SpineWidget>();
        }
    }
}
