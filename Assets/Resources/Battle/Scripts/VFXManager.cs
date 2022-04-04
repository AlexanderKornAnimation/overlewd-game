using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace Overlewd
{
    public class VFXManager : MonoBehaviour
    {

        [SerializeField] private string path = "Battle/";
        [SerializeField] private string animationName = "action";
        [SerializeField] private bool loop = false;

        public float delay = 0f;
        public float duration = 1f;
        public SpineWidget spineWiget;

        private void Start()
        {
            //UITools.SetStretch(GetComponent<RectTransform>());
            spineWiget = SpineWidget.GetInstance(transform);
            //duration = spineWiget.GetAnimationDuaration(animationName);
            spineWiget.Initialize(path);
            StartCoroutine(StartAfterDelay());
        }

        IEnumerator StartAfterDelay()
        {
            yield return new WaitForSeconds(delay);
            spineWiget.PlayAnimation(animationName, loop);
            yield return new WaitForSeconds(duration);
            Destroy(this.gameObject);
        }
    }
}