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

        public float delay = 0f; //predelay
        public float duration = 1f; //ms
        private SpineWidget spineWiget;
        [SerializeField] private GameObject spineVFXPrefab;

        private void Start()
        {
            var parent = GetComponentInParent<Transform>().localScale;
            //UITools.SetStretch(GetComponent<RectTransform>());
            if (spineVFXPrefab) 
                spineWiget = SpineWidget.GetInstance(spineVFXPrefab, transform);
            else
                spineWiget = SpineWidget.GetInstance(path, transform);

            duration = spineWiget.GetAnimationDuaration(animationName);
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