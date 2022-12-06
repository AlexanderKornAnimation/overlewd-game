using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class UIfx
    {
        public const string UIFX_QUEST_BOOK01 = "uifx_quest_book01";
        public const string UIFX_LVLUP01 = "uifx_lvlup01";
        public const string UIFX_LVLUP02 = "uifx_lvlup02";

        public const string UIFX_L_CLICK = "";

        public static void Inst(string animDataTitle, Transform parent, Vector2 offset = default(Vector2))
        {
            var vfx = SpineWidget.GetInstanceDisposable(GameData.animations[animDataTitle], parent);
            vfx.transform.localPosition += (Vector3)offset;
        }

        public static void InstLocal(string prefabPath, Transform parent, Vector2 offset = default(Vector2))
        {
            var vfx = SpineWidget.GetInstanceDisposable(prefabPath, parent);
            vfx.transform.localPosition += (Vector3)offset;
        }
    }
}
