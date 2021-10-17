using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class ImmediateShow : BaseScreenTrasition
    {
        void Awake()
        {
            
        }

        void Update()
        {
            Destroy(this);
        }
    }
}
