using UnityEngine;
using UnityEngine.EventSystems;
namespace Overlewd
{
    public class DebugButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        BattleManager bm;
        float timer = 2.5f, time = 0f;
        bool press, canOpen = false;


        private void Awake() => bm = FindObjectOfType<BattleManager>();

        public void OnPointerDown(PointerEventData eventData)
        {
            time = 0f;
            press = true;
            //canOpen = false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (canOpen || bm.debug > 0) bm.PressDebug();
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
    }
}