using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public abstract class MissclickTransition : MonoBehaviour
    {
        protected CanvasGroup canvasGroup;
        protected bool localCanvasGroup = false;

        protected virtual void Awake()
        {
            canvasGroup = gameObject.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
                localCanvasGroup = true;
            }

            UIManager.AddUserInputLocker(new UserInputLocker(this));
        }

        protected virtual void OnDestroy()
        {
            if (localCanvasGroup)
            {
                Destroy(canvasGroup);
            }

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
