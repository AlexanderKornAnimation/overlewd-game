using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public abstract class MissclickTransition : MonoBehaviour
    {
        protected virtual void Awake()
        {
            UIManager.PushUserInputLocker(new UserInputLocker(this));
        }

        protected virtual void OnDestroy()
        {
            UIManager.PopUserInputLocker(new UserInputLocker(this));
        }

        public virtual async Task ProgressAsync()
        {
            await Task.CompletedTask;
        }
    }

    public abstract class MissclickShow : MissclickTransition
    {

    }
    public abstract class MissclickHide : MissclickTransition
    {

    }
}
