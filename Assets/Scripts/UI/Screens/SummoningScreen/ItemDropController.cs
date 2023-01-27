using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Overlewd
{
    namespace NSSummoningScreen
    {
        public class ItemDropController : MonoBehaviour
        {
            Animator ani;
            Transform anchor;
            Button button;

            public GameObject memoShape;
            public GameObject equipShape;
            public GameObject battleGirlShape;

            private int grade = 0;
            public int maxGrade = 0;
            public bool canClick = false;
            public int shape = 0;
            private Image white;
            public Sprite[] whiteSprites;
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
                white = anchor.Find("white").GetComponent<Image>();
            }

            private void Start() => button?.onClick.AddListener(Play);

            public void SetUp(int shape, int maxGrade, Sprite sprite)
            {
                this.shape = shape;
                this.maxGrade = maxGrade;
                if (shape == 0)
                    spineWiget = SpineWidget.GetInstance(memoShape, anchor);
                else if (shape == 1)
                    spineWiget = SpineWidget.GetInstance(equipShape, anchor);
                else
                    spineWiget = SpineWidget.GetInstance(battleGirlShape, anchor);

                var persIcon = spineWiget.transform.Find("Mask/Item").GetComponent<Image>();
                persIcon.sprite = sprite;
                persIcon.SetNativeSize();

                spineWiget?.PlayAnimation("cr_grey", true);
                spineWiget?.transform.SetAsFirstSibling();
                if (delay > 0) StartCoroutine(SpineDelay());
                maskObj = spineWiget?.transform.Find("Mask").GetComponent<Image>();
                maskObj.color = new Color(1, 1, 1, 0);
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
                    white.sprite = whiteSprites[shape];
                    //if (shape == 0)
                        ani?.Play("Base Layer.SwitchShard");
                    //else if (shape == 1)
                    //    ani?.Play("Base Layer.SwitchEquip");
                    //else
                    //    ani?.Play("Base Layer.SwitchGirls");
                    canClick = false;
                }
            }

            public void DestroySpineWiget()
            {
                ani?.Play("Base Layer.Destroy");
                Destroy(spineWiget.gameObject, 0.99f);
            }

            public void CanClick()
            {
                if (grade <= maxGrade)
                {
                    canClick = true;
                    if (parentDE.mat.Count > 0)
                        partMat.material = parentDE?.mat[Mathf.Max(0, grade - 1)];
                }
            }
            public void UpgradeOrOpen()
            {
                if (grade == maxGrade)
                {
                    if (parentDE.landParticles[grade] != null && grade > 0)
                        Instantiate(parentDE.landParticles[grade - 1], anchor);
                    maskObj.color = maskVal;
                    parentDE.ShardIsOpen();
                }
                else
                {
                    string animationName = "cr_green";
                    if (grade == 1)
                        animationName = "cr_violet";
                    else if (grade >= 2)
                        animationName = "cr_gold";
                    spineWiget?.PlayAnimation(animationName, true);
                }
                grade++;
            }
        }
    }
}