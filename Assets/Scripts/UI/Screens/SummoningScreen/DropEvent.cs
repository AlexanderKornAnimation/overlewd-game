using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class DropEvent : MonoBehaviour
    {
        Animator ani;
        public List<ItemDropController> items;
        public List<Material> mat;
        public List<GameObject> landParticles = null;

        private void Awake()
        {
            ani = GetComponent<Animator>();
        }
        public void DisableAnimator()
        {
            if (ani) ani.enabled = false;
            if (items != null)
                foreach (var i in items)
                {
                    i.canClick = true;
                    i.parentDE = this;
                }
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(
                    UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            }
        }
    }
}