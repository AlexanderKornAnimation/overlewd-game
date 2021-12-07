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

        void Start()
        {
            var resources = ResourceManager.GetResourcesFileNames();
            foreach (var resName in resources)
            {
                var texture = ResourceManager.LoadTexture(resName);
                loadedTextures.Add(texture);
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

            //spine test
            var anim_sex = SpineWidget.GetInstance(transform);
            anim_sex.Initialize("Spine/Ulvi/sex/idle01_SkeletonData", true);
            anim_sex.PlayAnimation("idle", true);

            var anim_ui = SpineWidget.GetInstance(transform);
            anim_ui.Initialize("Spine/Ulvi/ui/idle_SkeletonData", false);
            anim_ui.PlayAnimation("idle", true);

            var anim_emotion = SpineWidget.GetInstance(transform);
            anim_emotion.Initialize("Spine/Ulvi/emotions/idle_SkeletonData", false);
            anim_emotion.PlayAnimation("idle", true);
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
