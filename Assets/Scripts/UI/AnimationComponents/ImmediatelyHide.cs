using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class ImmediatelyHide : UIAnimationBase
    {

        void Awake()
        {
            
        }

        void Start()
        {

        }

        void Update()
        {
            Destroy(gameObject);
        }
    }

}
