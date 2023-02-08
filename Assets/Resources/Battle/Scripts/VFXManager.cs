using System.Collections;
using UnityEngine;


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
        public void Create(string name, Transform spawnPoint, float preDelay = 0f, bool invertX = false)
        {
            delay = preDelay;
            spineWidget = SpineWidget.GetInstance(GameData.animations.GetByTitle(name), spawnPoint);
            if (spineWidget != null)
            {
                duration = spineWidget.GetAnimationDuaration(animationName);
                if (invertX)
                    spineWidget.GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                FindObjectOfType<BattleManager>().log.Add($"Can't load Spine VFX of Animation {name} from VFXManager", true);
                Destroy(gameObject);
                return;
            }

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
            Destroy(gameObject);
        }
    }
}