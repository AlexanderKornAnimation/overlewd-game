using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public abstract class MissclickTransition : MonoBehaviour
    {
        protected RectTransform missclickRectTransform;

        protected float duration = 0.3f;
        protected float time = 0.0f;

        protected virtual void Awake()
        {
            missclickRectTransform = GetComponent<RectTransform>();
        }
    }

    public abstract class MissclickShow : MissclickTransition
    {

    }
    public abstract class MissclickHide : MissclickTransition
    {

    }
}
