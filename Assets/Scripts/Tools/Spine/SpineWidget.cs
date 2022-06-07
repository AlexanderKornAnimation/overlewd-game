using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using System;
using Spine;
using System.Linq;

namespace Overlewd
{
    public class SpineWidget : BaseWidget
    {
        public event Action startListeners;
        public event Action completeListeners;

        private class SpineEventInfo
        {
            public string eventName;
            public event Action listeners;
            public void CallListeners()
            {
                listeners?.Invoke();
            }
            public bool HasListener(Action listener)
            {
                return listener.
                    GetInvocationList().
                    ToList().
                    Exists(l => l == (MulticastDelegate)listener);
            }
        }
        private List<SpineEventInfo> eventListeners = new List<SpineEventInfo>();

        private SkeletonGraphic skeletonGraphic;

        public void Initialize()
        {
            skeletonGraphic = GetComponent<SkeletonGraphic>();
            skeletonGraphic.Initialize(false);

            skeletonGraphic.AnimationState.Start += StartListener;
            skeletonGraphic.AnimationState.Complete += CompleteListener;
            skeletonGraphic.AnimationState.Event += EventListener;
        }


        private void StartListener(TrackEntry e)
        {
            startListeners?.Invoke();
        }

        private void CompleteListener(TrackEntry e)
        {
            completeListeners?.Invoke();
        }

        public void AddEventListener(string eventName, Action listener)
        {
            var eventInfo = eventListeners.Find(item => item.eventName == eventName);
            if (eventInfo != null)
            {
                if (!eventInfo.HasListener(listener))
                {
                    eventInfo.listeners += listener;
                }
            }
            else
            {
                var newEventInfo = new SpineEventInfo { eventName = eventName };
                newEventInfo.listeners += listener;
                eventListeners.Add(newEventInfo);
            }
        }

        public void RemoveEventListeners(string eventName)
        {
            eventListeners.RemoveAll(item => item.eventName == eventName);
        }

        public void RemoveEventListener(Action listener)
        {
            foreach (var eventInfoItem in eventListeners)
            {
                if (eventInfoItem.HasListener(listener))
                {
                    eventInfoItem.listeners -= listener;
                }
            }
        }

        private void EventListener(TrackEntry trackEntry, Spine.Event e)
        {
            foreach (var eventInfoItem in eventListeners.FindAll(item => item.eventName == e.Data.Name))
            {
                eventInfoItem.CallListeners();
            }
        }

        public void PlayAnimation(string animationName, bool loop)
        {
            if (skeletonGraphic == null)
                return;
            skeletonGraphic.AnimationState.SetAnimation(0, animationName.Trim(), loop);
        }

        public void Pause()
        {
            if (skeletonGraphic == null)
                return;
            skeletonGraphic.freeze = true;
        }

        public void Play()
        {
            if (skeletonGraphic == null)
                return;
            skeletonGraphic.freeze = false;
        }

        public void FlipX()
        {
            transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
        }

        public void Scale(float scale)
        {
            transform.localScale = Vector3.Scale(transform.localScale, new Vector3(scale, scale, 1));
        }

        public float GetAnimationDuaration(string animationName)
        {
            return skeletonGraphic.SkeletonData.Animations.Find(a => a.Name == animationName).Duration;
        }

        public void Attach(GameObject obj, string boneName)
        {
            obj.transform.SetParent(transform);
            var boneFollower = obj.GetComponent<BoneFollowerGraphic>() ?? obj.AddComponent<BoneFollowerGraphic>();
            boneFollower.SkeletonGraphic = skeletonGraphic;
            boneFollower.SetBone(boneName);
        }

        public void Detach(GameObject obj)
        {
            obj?.transform.SetParent(null);
            Destroy(obj?.GetComponent<BoneFollowerGraphic>());
        }
        
        public static SpineWidget GetInstance(string prefabPath, Transform parent)
        {
            var inst = ResourceManager.InstantiateAsset<GameObject>(prefabPath, parent);
            var sw = inst?.AddComponent<SpineWidget>();
            sw?.Initialize();
            return sw;
        }

        public static SpineWidget GetInstance(GameObject obj, Transform parent)
        {
            var inst = Instantiate(obj, parent);
            var sw = inst?.AddComponent<SpineWidget>();
            sw?.Initialize();
            return sw;
        }

        public static SpineWidget GetInstance(string prefabPath, string assetBundleId, Transform parent)
        {
            var inst = ResourceManager.InstantiateRemoteAsset<GameObject>(prefabPath, assetBundleId, parent);
            var sw = inst?.AddComponent<SpineWidget>();
            sw?.Initialize();
            return sw;
        }
    }
}