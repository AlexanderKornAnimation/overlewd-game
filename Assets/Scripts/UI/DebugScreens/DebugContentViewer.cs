using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class DebugContentViewer : BaseScreen
    {
        private List<Texture2D> loadedTextures = new List<Texture2D>();
        private int currentTextureId;

        private Button prevBtn;
        private Button nextBtn;
        private Image image;

        IEnumerator Start()
        {
            var resources = ResourceManager.GetResourcesFileNames();
            foreach (var resName in resources)
            {
                yield return ResourceManager.LoadTextureByFileName(resName, texture =>
                {
                    loadedTextures.Add(texture);
                });
            }

            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/DebugScreens/DebugContentViewer/ContentViewer"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            image = screenRectTransform.Find("Canvas").Find("Image").GetComponent<Image>();

            screenRectTransform.Find("Canvas").Find("Castle").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<CastleScreen>();
            });

            screenRectTransform.Find("Canvas").Find("ScreenViewer").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<DebugScreenViewer>();
            });

            if (loadedTextures.Count > 0)
            {
                SetSprite();
            }

            prevBtn = screenRectTransform.Find("Canvas").Find("PrevBtn").GetComponent<Button>();
            prevBtn.onClick.AddListener(() =>
            {
                currentTextureId--;
                CheckButtons();
                SetSprite();
            });

            nextBtn = screenRectTransform.Find("Canvas").Find("NextBtn").GetComponent<Button>();
            nextBtn.onClick.AddListener(() =>
            {
                currentTextureId++;
                CheckButtons();
                SetSprite();
            });

            CheckButtons();

            //load spine
            var spineGO = new GameObject("spine_test");
            var spineGO_rt = spineGO.AddComponent<RectTransform>();
            spineGO_rt.SetParent(transform, false);

            var skeletonDataAsset = Resources.Load<Spine.Unity.SkeletonDataAsset>("Spine/Ulvi/idle_SkeletonData");
            var spineGO_sg = spineGO.AddComponent<Spine.Unity.SkeletonGraphic>();
            spineGO_sg.skeletonDataAsset = skeletonDataAsset;
            spineGO_sg.Initialize(false);
            spineGO_sg.AnimationState.SetAnimation(0, "idle", true);
        }

        private void SetSprite()
        {
            var texture = loadedTextures[currentTextureId];
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            image.sprite = sprite;
        }

        private void CheckButtons()
        {
            if (loadedTextures.Count > 0)
            {
                if (currentTextureId == 0)
                {
                    prevBtn.gameObject.SetActive(false);
                    nextBtn.gameObject.SetActive(true);
                }
                else if (currentTextureId == loadedTextures.Count - 1)
                {
                    prevBtn.gameObject.SetActive(true);
                    nextBtn.gameObject.SetActive(false);
                }
                else
                {
                    prevBtn.gameObject.SetActive(true);
                    nextBtn.gameObject.SetActive(true);
                }
            }
            else
            {
                prevBtn.gameObject.SetActive(false);
                nextBtn.gameObject.SetActive(false);
            }
        }

        void Update()
        {

        }
    }
}
