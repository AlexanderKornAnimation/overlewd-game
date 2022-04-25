using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSPrepareBattlePopup
    {
        public abstract class BaseCharacter : MonoBehaviour
        {
            public AdminBRO.Character characterData { get; set; }

            protected Transform canvas;

            protected Image art;
            protected TextMeshProUGUI level;
            protected TextMeshProUGUI characterClass;

            protected virtual void Awake()
            {
                canvas = transform.Find("Canvas");
                art = canvas.Find("Art").GetComponent<Image>();
                level = canvas.Find("LevelBack").Find("Level").GetComponent<TextMeshProUGUI>();
                characterClass = canvas.Find("Class").GetComponent<TextMeshProUGUI>();
            }
            
            protected virtual void Start()
            {
                Customize();
            }

            protected virtual void Customize()
            {
                // level.text = characterData.level.HasValue ? characterData.level.Value.ToString() : "1";
                //
                // characterClass.text = characterData.characterClass switch
                // {
                //     AdminBRO.Character.Class_Assassin => AdminBRO.Character.Sprite_EnemyAssasin,
                //     AdminBRO.Character.Class_Bruiser => AdminBRO.Character.Sprite_EnemyBruiser,
                //     AdminBRO.Character.Class_Healer => AdminBRO.Character.Sprite_EnemyHealer,
                //     AdminBRO.Character.Class_Caster => AdminBRO.Character.Sprite_EnemyCaster,
                //     AdminBRO.Character.Class_Tank => AdminBRO.Character.Sprite_EnemyTank,
                //     _ => AdminBRO.Character.Sprite_EnemyAssasin
                // }; 
            }
        }
    }
}