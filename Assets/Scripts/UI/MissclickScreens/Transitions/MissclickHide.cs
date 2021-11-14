using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class MissclickHide : MonoBehaviour
    {
        private BaseMissclick missClickScreen;
        private float time = 0.0f;
        private float duration = 0.3f;

        void Awake()
        {
            missClickScreen = GetComponent<BaseMissclick>();
            missClickScreen.UpdateHide(0.0f);
        }

        void Update()
        {
            time += Time.deltaTime;
            if (time > duration)
            {
                missClickScreen.UpdateHide(1.0f);
                Destroy(gameObject);
            }
            else
            {
                missClickScreen.UpdateHide(time / duration);
            }
        }
    }
}
