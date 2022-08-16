using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Overlewd
{
    public class ItemDropController : MonoBehaviour
    {
        Animator ani;
        Transform anchor;
        Button button;
        public GameObject shardShape;
        public GameObject equipShape;
        private int grade = 0;
        public int maxGrade = 0;
        public bool canClick = false;
        public int shape = 0;
        private SpineWidget spineWiget;
        public float delay = 0f;
        public DropEvent parentDE;
        public UIParticleSystem partMat;
        public Color maskVal;
        private Image maskObj;

        private void Awake()
        {
            ani = GetComponent<Animator>();
            anchor = transform.Find("anchor");
            button = GetComponent<Button>();
            
        }

        private void Start()
        {
            button?.onClick.AddListener(Play);
            var mg = Random.Range(0, 4);
            //mg = 3;
            SetUp(shape, mg); //Удалить или закомментить после добавления
        }

        public void SetUp(int shape, int maxGrade)
        {
            spineWiget = shape == 0 ?
                SpineWidget.GetInstance(shardShape, anchor) :
                SpineWidget.GetInstance(equipShape, anchor);

            spineWiget?.PlayAnimation("cr_grey", true);
            spineWiget?.transform.SetSiblingIndex(0);
            if (delay > 0) StartCoroutine(SpineDelay());
            this.maxGrade = maxGrade;
            maskObj = spineWiget?.transform.Find("Mask").GetComponent<Image>();
            maskObj.color = new Color (1,1,1,0);
        }
        IEnumerator SpineDelay()
        {
            spineWiget?.Pause();
            yield return new WaitForSeconds(delay);
            spineWiget?.Play();
        }
        void Play()
        {
            if (canClick)
            {
                if (shape == 0)
                    ani?.Play("Base Layer.SwitchShard");
                else
                    ani?.Play("Base Layer.SwitchEquip");
                canClick = false;
            }
        }

        public void CanClick()
        {
            if (grade != maxGrade + 1) {
                canClick = true;
                partMat.material = parentDE.mat[grade-1];
            }
        }
        public void UpgradeOrOpen()
        {
            if (grade == maxGrade)
            {
                //Open shard create server based item

                /* Destroy(spineWiget.gameObject);
                //Or
                foreach (GameObject child in anchor.transform)
                    Destroy(child);
                Instantiate(shard_from_server, anchor);*/
                if (parentDE.landParticles[grade] != null && grade > 0)
                    Instantiate(parentDE.landParticles[grade-1], anchor);
                maskObj.color = maskVal;
                //Debug.Log("Open");
                grade++;
            }
            else
            {
                string animationName = "cr_green";
                if (grade == 1)
                    animationName = "cr_violet";
                else if (grade == 2)
                    animationName = "cr_gold";

                if (spineWiget)
                    spineWiget.PlayAnimation(animationName, true);
                grade++;
            }
        }
    }
}