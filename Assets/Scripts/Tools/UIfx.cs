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

        public static void Inst(string uifxAnimName, Transform parent, Vector2 offset = default(Vector2))
        {
            var vfx = SpineWidget.GetInstanceDisposable(GameData.animations[uifxAnimName], parent);
            vfx.transform.localPosition += (Vector3)offset;
        }
    }
}
