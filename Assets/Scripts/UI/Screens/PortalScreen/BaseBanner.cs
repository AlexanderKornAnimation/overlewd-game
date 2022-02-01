using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSPortalScreen
    {
        public abstract class BaseBanner : MonoBehaviour
        {
            public TierButtonsScroll tierButtonsScroll { get; set; }

            public event Action<BaseBanner> selectBanner;

            protected virtual void Awake()
            {

            }

            protected virtual void BannerClick()
            {
                SoundManager.PlayOneShoot(SoundManager.SoundPath.UI.ButtonClick);
                selectBanner?.Invoke(this);
            }

            public virtual void Select()
            {
                tierButtonsScroll?.Show();
            }

            public virtual void Deselect()
            {
                tierButtonsScroll?.Hide();
            }
        }
    }
}
