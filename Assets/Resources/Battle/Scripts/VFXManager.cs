using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Spine;


namespace Overlewd
{
    public class VFXManager : MonoBehaviour
    {
        [SerializeField] private string animationName = "action";

        private SpineWidget spineWidget;
        public float delay = 0f;
        public float duration = 1f;
        [SerializeField] private GameObject spineVFXPrefab = null;

        private void Start()
        {
            if (spineVFXPrefab != null) Create(spineVFXPrefab);
        }
        public float Setup(AdminBRO.Animation sw, Transform spawnPoint, float preDelay = 0f, bool invertX = false)
        {
            delay = preDelay;
            spineWidget = SpineWidget.GetInstance(sw, spawnPoint);
            if (spineWidget != null)
                duration = spineWidget.GetAnimationDuaration(animationName);
            else
                FindObjectOfType<BattleManager>().log.Add(sw.title + ": Data Corrupted", true);
            if (invertX)
                spineWidget.GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
            spineWidget?.PlayAnimation(animationName, false);
            spineWidget?.Pause();
            StartCoroutine(StartAfterDelay());
            return duration;
        }
        public void Create(GameObject sw)
        {
            spineWidget = SpineWidget.GetInstance(sw, transform);
            duration = spineWidget.GetAnimationDuaration(animationName);
            spineWidget?.PlayAnimation(animationName, false);
            spineWidget?.Pause();
            StartCoroutine(StartAfterDelay());
        }

        IEnumerator StartAfterDelay()
        {
            yield return new WaitForSeconds(delay);
            spineWidget?.Play();
            yield return new WaitForSeconds(duration);
            Destroy(spineWidget?.gameObject);
            Destroy(this.gameObject);
        }
    }
}