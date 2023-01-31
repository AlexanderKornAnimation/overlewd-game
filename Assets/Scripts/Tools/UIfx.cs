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
        public const string UIFX_OVERLORD_SPELLS = "uifx_overlord_spells";

        //laboratory
        public const string UIFX_COLB_BUBBLES = "uifx_colb_bubbles";
        public const string UIFX_COLB_TENTACLES = "uifx_colb_tentacles";

        public const string LOCAL_UIFX_SCREEN_TAP = "Prefabs/FX/uifx_tap/uifx_tap";
        public const string LOCAL_UIFX_SCREEN_TAP_PC = "Prefabs/FX/uifx_tap/uifx_tap_pc";
        public const string LOCAL_UIFX_TUTOR_HINT_TAP = "Prefabs/FX/uifx_notification/idle";

        public static SpineWidget Inst(string animDataTitle, Transform parent, Vector2 offset = default(Vector2))
        {
            var vfx = SpineWidget.GetInstance(GameData.animations[animDataTitle], parent);
            vfx.raycastTarget = false;
            vfx.transform.localPosition += (Vector3)offset;
            return vfx;
        }

        public static SpineWidget InstDisposable(string animDataTitle, Transform parent, Vector2 offset = default(Vector2))
        {
            var vfx = SpineWidget.GetInstance(GameData.animations[animDataTitle], parent);
            vfx.destroyAfterComplete = true;
            vfx.raycastTarget = false;
            vfx.transform.localPosition += (Vector3)offset;
            return vfx;
        }

        public static SpineWidget InstLocal(string prefabPath, Transform parent, Vector2 offset = default(Vector2))
        {
            var vfx = SpineWidget.GetInstance(prefabPath, parent);
            vfx.raycastTarget = false;
            vfx.transform.localPosition += (Vector3)offset;
            return vfx;
        }

        public static SpineWidget InstLocalDisposable(string prefabPath, Transform parent, Vector2 offset = default(Vector2))
        {
            var vfx = SpineWidget.GetInstance(prefabPath, parent);
            vfx.destroyAfterComplete = true;
            vfx.raycastTarget = false;
            vfx.transform.localPosition += (Vector3)offset;
            return vfx;
        }
    }
}
