using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public static class GameLocalResources
    {
        //FTUE
        public static Dictionary<string, Dictionary<string, string>> mainSexAnimPath = new Dictionary<string, Dictionary<string, string>>
        {
            ["MainSex1"] = new Dictionary<string, string>
            {
                ["back"] = "FTUE/MainSexAnims/MainSex1/back_SkeletonData",
                ["idle"] = "FTUE/MainSexAnims/MainSex1/idle01_SkeletonData"
            },
            ["FinalSex1"] = new Dictionary<string, string>
            {
                ["back"] = "FTUE/MainSexAnims/FinalSex1/back_SkeletonData",
                ["idle"] = "FTUE/MainSexAnims/FinalSex1/idle01_SkeletonData"
            },

            ["MainSex2"] = new Dictionary<string, string>
            {
                ["back"] = "FTUE/MainSexAnims/MainSex2/back_SkeletonData",
                ["idle"] = "FTUE/MainSexAnims/MainSex2/idle01_SkeletonData"
            },
            ["FinalSex2"] = new Dictionary<string, string>
            {
                ["back"] = "FTUE/MainSexAnims/FinalSex2/back_SkeletonData",
                ["idle"] = "FTUE/MainSexAnims/FinalSex2/idle01_SkeletonData"
            },

            ["MainSex3"] = new Dictionary<string, string>
            {
                ["back"] = "FTUE/MainSexAnims/MainSex3/back_SkeletonData",
                ["idle"] = "FTUE/MainSexAnims/MainSex3/idle01_SkeletonData"
            },
            ["FinalSex3"] = new Dictionary<string, string>
            {
                ["back"] = "FTUE/MainSexAnims/FinalSex3/back_SkeletonData",
                ["idle"] = "FTUE/MainSexAnims/FinalSex3/idle01_SkeletonData"
            },
        };

        public static Dictionary<string, Dictionary<string, string>> cutInAnimPath = new Dictionary<string, Dictionary<string, string>>
        {
            ["CutIn1"] = new Dictionary<string, string>
            {
                ["back"] = "FTUE/CutInAnims/CutIn1/back_SkeletonData",
                ["idle"] = "FTUE/CutInAnims/CutIn1/idle01_SkeletonData"
            },
            ["CutIn2"] = new Dictionary<string, string>
            {
                ["back"] = "FTUE/CutInAnims/CutIn2/back_SkeletonData",
                ["idle"] = "FTUE/CutInAnims/CutIn2/idle01_SkeletonData"
            },
            ["CutIn3"] = new Dictionary<string, string>
            {
                ["back"] = "FTUE/CutInAnims/CutIn3/back_SkeletonData",
                ["idle"] = "FTUE/CutInAnims/CutIn3/idle_SkeletonData"
            },
            ["CutIn4"] = new Dictionary<string, string>
            {
                ["back"] = "FTUE/CutInAnims/CutIn4/back_overlord_SkeletonData"
            },
            ["CutIn5"] = new Dictionary<string, string>
            {
                ["back"] = "FTUE/CutInAnims/CutIn5/back_SkeletonData",
                ["idle"] = "FTUE/CutInAnims/CutIn5/idle01_SkeletonData"
            },
            ["CutIn6"] = new Dictionary<string, string>
            {
                ["back"] = "FTUE/CutInAnims/CutIn6/back_SkeletonData",
                ["idle"] = "FTUE/CutInAnims/CutIn6/idle01_SkeletonData"
            },
            ["CutIn7"] = new Dictionary<string, string>
            {
                ["back"] = "FTUE/CutInAnims/CutIn7/back_SkeletonData",
                ["idle"] = "FTUE/CutInAnims/CutIn7/idle01_SkeletonData"
            },
            ["CutIn8"] = new Dictionary<string, string>
            {
                ["back"] = "FTUE/CutInAnims/CutIn8/back_SkeletonData",
                ["idle"] = "FTUE/CutInAnims/CutIn8/idle01_SkeletonData"
            },
            ["CutIn9"] = new Dictionary<string, string>
            {
                ["back"] = "FTUE/CutInAnims/CutIn9/back_SkeletonData",
                ["idle"] = "FTUE/CutInAnims/CutIn9/idle01_SkeletonData"
            },
        };

        public static Dictionary<string, Dictionary<string, string>> emotionsAnimPath = new Dictionary<string, Dictionary<string, string>>
        {
            [AdminBRO.DialogCharacterKey.Overlord] = new Dictionary<string, string>
            {
                [AdminBRO.DialogCharacterAnimation.Angry] = "FTUE/Emotions/Overlord/angry_SkeletonData",
                [AdminBRO.DialogCharacterAnimation.Happy] = "FTUE/Emotions/Overlord/happy_SkeletonData",
                [AdminBRO.DialogCharacterAnimation.Idle] = "FTUE/Emotions/Overlord/idle_SkeletonData",
                [AdminBRO.DialogCharacterAnimation.Love] = "FTUE/Emotions/Overlord/love_SkeletonData",
                [AdminBRO.DialogCharacterAnimation.Surprised] = "FTUE/Emotions/Overlord/surprised_SkeletonData",
            },
            [AdminBRO.DialogCharacterKey.Ulvi] = new Dictionary<string, string>
            {
                [AdminBRO.DialogCharacterAnimation.Angry] = "FTUE/Emotions/Ulvi/angry_SkeletonData",
                [AdminBRO.DialogCharacterAnimation.Happy] = "FTUE/Emotions/Ulvi/happy_SkeletonData",
                [AdminBRO.DialogCharacterAnimation.Idle] = "FTUE/Emotions/Ulvi/idle_SkeletonData",
                [AdminBRO.DialogCharacterAnimation.Love] = "FTUE/Emotions/Ulvi/love_SkeletonData",
                [AdminBRO.DialogCharacterAnimation.Surprised] = "FTUE/Emotions/Ulvi/surprised_SkeletonData",
            },
            [AdminBRO.DialogCharacterKey.UlviWolf] = new Dictionary<string, string>
            {
                [AdminBRO.DialogCharacterAnimation.Angry] = "FTUE/Emotions/UlviFurry/angry_SkeletonData",
                [AdminBRO.DialogCharacterAnimation.Happy] = "FTUE/Emotions/UlviFurry/happy_SkeletonData",
                [AdminBRO.DialogCharacterAnimation.Idle] = "FTUE/Emotions/UlviFurry/idle_SkeletonData",
                [AdminBRO.DialogCharacterAnimation.Love] = "FTUE/Emotions/UlviFurry/love_SkeletonData",
                [AdminBRO.DialogCharacterAnimation.Surprised] = "FTUE/Emotions/UlviFurry/surprised_SkeletonData",
            },
            [AdminBRO.DialogCharacterKey.Faye] = new Dictionary<string, string>
            {
                [AdminBRO.DialogCharacterAnimation.Angry] = null,
                [AdminBRO.DialogCharacterAnimation.Happy] = null,
                [AdminBRO.DialogCharacterAnimation.Idle] = null,
                [AdminBRO.DialogCharacterAnimation.Love] = null,
                [AdminBRO.DialogCharacterAnimation.Surprised] = null,
            },
            [AdminBRO.DialogCharacterKey.Adriel] = new Dictionary<string, string>
            {
                [AdminBRO.DialogCharacterAnimation.Angry] = "FTUE/Emotions/Adriel/angry_SkeletonData",
                [AdminBRO.DialogCharacterAnimation.Happy] = "FTUE/Emotions/Adriel/happy_SkeletonData",
                [AdminBRO.DialogCharacterAnimation.Idle] = "FTUE/Emotions/Adriel/idle_SkeletonData",
                [AdminBRO.DialogCharacterAnimation.Love] = "FTUE/Emotions/Adriel/love_SkeletonData",
                [AdminBRO.DialogCharacterAnimation.Surprised] = "FTUE/Emotions/Adriel/surprised_SkeletonData",
            },
        };
        //

        public static Dictionary<string, string> dialogCharacterPrefabPath = new Dictionary<string, string>
        {
            [AdminBRO.DialogCharacterKey.Overlord] = "Prefabs/UI/Screens/DialogScreen/Overlord",
            [AdminBRO.DialogCharacterKey.Ulvi] = "Prefabs/UI/Screens/DialogScreen/Ulvi",
            [AdminBRO.DialogCharacterKey.UlviWolf] = "Prefabs/UI/Screens/DialogScreen/UlviFurry",
            [AdminBRO.DialogCharacterKey.Faye] = "Prefabs/UI/Screens/DialogScreen/Faye",
            [AdminBRO.DialogCharacterKey.Adriel] = "Prefabs/UI/Screens/DialogScreen/Adriel"
        };

        public static Dictionary<string, string> uiSoundEventPath = new Dictionary<string, string>()
        {
            ["Test"] = "event:/test"
        };
    }

}
