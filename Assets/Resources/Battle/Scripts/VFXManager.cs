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
        public float scale = 1f;
        private SpineWidget spineWiget;
        [SerializeField] private GameObject vfxPrefab;

        private void Start()
        {
            transform.localScale *= scale;
            //UITools.SetStretch(GetComponent<RectTransform>());
            spineWiget = SpineWidget.GetInstance(vfxPrefab, transform);
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