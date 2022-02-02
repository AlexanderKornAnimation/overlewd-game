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

        private Button castleBtn;
        private Button screenViewerBtn;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/DebugScreens/DebugContentViewer/ContentViewer", transform);

            var canvas = screenInst.transform.Find("Canvas");

            image = canvas.Find("Image").GetComponent<Image>();

            castleBtn = canvas.Find("Castle").GetComponent<Button>();
            castleBtn.onClick.AddListener(CastleButtonClick);
            screenViewerBtn = canvas.Find("ScreenViewer").GetComponent<Button>();
            screenViewerBtn.onClick.AddListener(ScreenViewerButtonClick);

            prevBtn = canvas.Find("PrevBtn").GetComponent<Button>();
            prevBtn.onClick.AddListener(PrevButtonClick);

            nextBtn = canvas.Find("NextBtn").GetComponent<Button>();
            prevBtn.onClick.AddListener(NextButtonClick);
        }

        void Start()
        {
            var resources = ResourceManager.GetResourcesFileNames();
            foreach (var resName in resources)
            {
                var texture = ResourceManager.LoadTexture(resName);
                loadedTextures.Add(texture);
            }

            if (loadedTextures.Count > 0)
            {
                SetSprite();
            }

            CheckButtons();
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

        private void CastleButtonClick()
        {
            UIManager.ShowScreen<CastleScreen>();
        }

        private void ScreenViewerButtonClick()
        {
            UIManager.ShowScreen<DebugScreenViewer>();
        }

        private void PrevButtonClick()
        {
            currentTextureId--;
            CheckButtons();
            SetSprite();
        }

        private void NextButtonClick()
        {
            currentTextureId++;
            CheckButtons();
            SetSprite();
        }
    }
}
