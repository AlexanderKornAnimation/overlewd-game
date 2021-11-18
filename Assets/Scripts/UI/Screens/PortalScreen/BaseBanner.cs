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
            public PortalScreen portalScreen { get; set; }
            public TierButtonsScroll tierButtonsScroll { get; set; }

            protected virtual void Awake()
            {

            }

            protected virtual void BannerClick()
            {
                portalScreen?.SelectBanner(this);
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
