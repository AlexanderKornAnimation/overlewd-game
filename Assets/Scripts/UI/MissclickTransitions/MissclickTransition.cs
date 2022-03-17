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

        protected float deltaTimeInc { 
            get 
            {
                return Time.deltaTime > 1.0f / 60.0f ? 1.0f / 60.0f : Time.deltaTime;
            } 
        }

        protected virtual void Awake()
        {
            missclickRectTransform = GetComponent<RectTransform>();

            UIManager.AddUserInputLocker(new UserInputLocker(this));
        }

        protected virtual void OnDestroy()
        {
            UIManager.RemoveUserInputLocker(new UserInputLocker(this));
        }
    }

    public abstract class MissclickShow : MissclickTransition
    {

    }
    public abstract class MissclickHide : MissclickTransition
    {

    }
}
