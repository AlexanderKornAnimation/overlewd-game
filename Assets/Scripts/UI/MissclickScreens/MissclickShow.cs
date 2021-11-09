using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class MissclickShow : MonoBehaviour
    {
        private BaseMissclick missClickScreen;
        private float time = 0.0f;
        private float duration = 0.3f;

        void Awake()
        {
            missClickScreen = GetComponent<BaseMissclick>();
            missClickScreen.UpdateShow(0.0f);
        }

        void Update()
        {
            time += Time.deltaTime;
            if (time > duration)
            {
                missClickScreen.UpdateShow(1.0f);
                Destroy(this);
            }
            else
            {
                missClickScreen.UpdateShow(time / duration);
            }
        }
    }
}
