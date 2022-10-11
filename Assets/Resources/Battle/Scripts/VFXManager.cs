using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace Overlewd
{
    public class VFXManager : MonoBehaviour
    {
        [SerializeField] private string animationName = "action";

        private SpineWidget spineWidget;
        public float delay = 0f;
        public float duration = 1f;
        [SerializeField] private GameObject spineVFXPrefab;
        
        public float Setup(AdminBRO.Animation sw, Transform spawnPoint, float preDelay = 0f)
        {
            delay = preDelay;
            spineWidget = SpineWidget.GetInstance(sw, spawnPoint);
            duration = spineWidget.GetAnimationDuaration(animationName);
            spineWidget.PlayAnimation(animationName, false);
            spineWidget.Pause();
            StartCoroutine(StartAfterDelay());
            return duration;
        }

        IEnumerator StartAfterDelay()
        {
            yield return new WaitForSeconds(delay);
            spineWidget.Play();
            yield return new WaitForSeconds(duration);
            Destroy(spineWidget.gameObject);
            Destroy(this.gameObject);
        }
    }
}