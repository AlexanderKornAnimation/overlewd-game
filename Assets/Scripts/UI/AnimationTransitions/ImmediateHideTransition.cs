using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class ImmediateHide : BaseScreenTrasition
    {
        void Awake()
        {

        }

        void Update()
        {
            Destroy(gameObject);
        }
    }
}
