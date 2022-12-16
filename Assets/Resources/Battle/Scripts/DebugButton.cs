using UnityEngine;
using UnityEngine.EventSystems;
namespace Overlewd
{
    public class DebugButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        BattleManager bm;
        float timer = 2f, time = 0f;
        bool press, canOpen = false;
        [SerializeField]
        GameObject[] turnOnGuiElements;
        [SerializeField]
        GameObject aniVFXTestGO;
        VFXTest aniVFXTest = null;

        private void Awake()
        {
            bm = FindObjectOfType<BattleManager>();
            
            foreach (var item in turnOnGuiElements)
                item.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            time = 0f;
            press = true;
            //canOpen = false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (canOpen || bm.debug > 0)
            {
                foreach (var item in turnOnGuiElements)
                    item.SetActive(true);
                bm.PressDebug();
            }
            if (bm.debug == 0)
            {
                foreach (var item in turnOnGuiElements)
                    item.SetActive(false);
                if (aniVFXTest != null)
                    Destroy(aniVFXTest.gameObject);
            }
            if (bm.debug == 3)
            {
                CreateVFXTest();
            }
            time = 0f;
            press = false;
        }

        private void Update()
        {
            if (press && !canOpen)
            {
                time += Time.deltaTime;
                if (time > timer) canOpen = true;
            }
        }
        void CreateVFXTest()
        {
            if (aniVFXTest == null)
                aniVFXTest = Instantiate(aniVFXTestGO, bm.transform).GetComponent<VFXTest>();
        }
    }
}