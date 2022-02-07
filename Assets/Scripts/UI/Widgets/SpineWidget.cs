using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using System;
using Spine;

namespace Overlewd
{
    public class SpineWidget : BaseWidget
    {
        public event Action startListeners;
        public event Action completeListeners;

        private SkeletonDataAsset skeletonDataAsset;
        private SkeletonGraphic skeletonGraphic;

        private void OnDestroy()
        {
            Destroy(skeletonDataAsset);
        }

        private void Initialize(string skeletonDataPath, bool multipleRenderCanvas, string assetBundleId)
        {
            skeletonDataAsset = String.IsNullOrEmpty(assetBundleId) ?
                ResourceManager.InstantiateAsset<SkeletonDataAsset>(skeletonDataPath) :
                ResourceManager.InstantiateRemoteAsset<SkeletonDataAsset>(skeletonDataPath, assetBundleId);
            skeletonGraphic = gameObject.AddComponent<SkeletonGraphic>();
            skeletonGraphic.allowMultipleCanvasRenderers = multipleRenderCanvas;
            skeletonGraphic.skeletonDataAsset = skeletonDataAsset;
            skeletonGraphic.Initialize(false);

            skeletonGraphic.AnimationState.Start += StartListener;
            skeletonGraphic.AnimationState.Complete += CompleteListener;
        }

        public void Initialize(string skeletonDataPath, bool multipleRenderCanvas = false)
        {
            Initialize(skeletonDataPath, multipleRenderCanvas, null);
        }

        public void Initialize(string skeletonDataPath, string assetBundleId, bool multipleRenderCanvas = false)
        {
            Initialize(skeletonDataPath, multipleRenderCanvas, assetBundleId);
        }

        private void StartListener(TrackEntry e)
        {
            startListeners?.Invoke();
        }

        private void CompleteListener(TrackEntry e)
        {
            completeListeners?.Invoke();
        }

        public void PlayAnimation(string animationName, bool loop)
        {
            skeletonGraphic.AnimationState.SetAnimation(0, animationName, loop);
        }

        public void Pause()
        {
            skeletonGraphic.freeze = true;
        }

        public void Play()
        {
            skeletonGraphic.freeze = false;
        }
        
        public static SpineWidget GetInstance(Transform parent)
        {
            var spineGO = new GameObject(nameof(SpineWidget));
            var spineGO_rt = spineGO.AddComponent<RectTransform>();
            spineGO_rt.SetParent(parent, false);
            return spineGO.AddComponent<SpineWidget>();
        }
    }
}
