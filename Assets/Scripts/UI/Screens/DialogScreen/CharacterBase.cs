using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSDialogScreen
    {
        public class CharacterBase : MonoBehaviour
        {
            public int? characterId { get; set; }
            public AdminBRO.DialogCharacter characterData =>
                GameData.dialogs.GetCharacterById(characterId);
            public int? skinId { get; set; }
            public AdminBRO.DialogCharacterSkin skinData =>
                GameData.dialogs.GetSkinById(skinId);

            public Image skin =>
                transform.GetComponentInChildren<Image>();
            public Sprite skinSprite
            {
                get => skin.sprite;
                set
                {
                    skin.sprite = value;
                    skin.SetNativeSize();
                    var skinRect = skin.rectTransform.rect;
                    var rootRect = transform as RectTransform;
                    rootRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, skinRect.width);
                    rootRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, skinRect.height);
                }
            }

            void Start()
            {
                skinSprite = ResourceManager.LoadSprite(skinData?.characterSkinImage);
            }

            public void Select()
            {
                skin.color = Color.white;
            }

            public void Deselect()
            {
                skin.color = Color.gray;
            }

            public void Hide()
            {
                Destroy(gameObject);
            }
        }
    }
}
