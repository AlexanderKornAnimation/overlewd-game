using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class ImmediatelyShow : UIAnimationBase
    {
        
        void Awake()
        {
            
        }

        void Start()
        {

        }

        void Update()
        {
            Destroy(this);
        }
    }

}
