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

        public SpineWidget SetAnimationName(string animationName)
        {
            animName = animationName;
            return this;
        }

        public SpineWidget SetMultipleRenderCanvas(bool multiple_rc)
        {
            multipleRenderCanvas = multiple_rc;
            return this;
        }

        public static SpineWidget CreateInstance(string skeletonDataPath, Transform parent)
        {
            var spineGO = new GameObject(nameof(SpineWidget));
            var spineGO_rt = spineGO.AddComponent<RectTransform>();
            spineGO_rt.SetParent(parent, false);
            var spineWidgetInst = spineGO.AddComponent<SpineWidget>();
            spineWidgetInst.skeletonDataPath = skeletonDataPath;
            return spineWidgetInst;
        }
    }
}
