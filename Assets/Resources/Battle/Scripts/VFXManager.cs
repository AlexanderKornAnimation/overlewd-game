using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace Overlewd
{
    public class VFXManager : MonoBehaviour
    {
        [SerializeField] private string animationName = "action";
        [SerializeField] private bool loop = false;

        private SpineWidget spineWiget;
        public float delay = 0f;
        public float duration = 1f;
        [SerializeField] private GameObject spineVFXPrefab;
        
        public float Setup(AdminBRO.Animation sw, Transform spawnPoint, float preDelay = 0f)
        {
            delay = preDelay;
            spineWiget = SpineWidget.GetInstance(sw, spawnPoint);
            StartCoroutine(StartAfterDelay());
            duration = spineWiget.GetAnimationDuaration(animationName);
            return duration;
        }

        IEnumerator StartAfterDelay()
        {
            yield return new WaitForSeconds(delay);
            spineWiget.PlayAnimation(animationName, loop);
            yield return new WaitForSeconds(duration);
            Destroy(spineWiget.gameObject);
            Destroy(this.gameObject);
        }
    }
}