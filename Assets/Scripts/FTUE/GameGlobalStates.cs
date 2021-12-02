using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace FTUE
    {
        public static class GameGlobalStates
        {
            public static int sexScreen_DialogId;
            public static AdminBRO.Dialog sexScreen_DialogData
            {
                get
                {
                    return GameData.GetDialogById(sexScreen_DialogId);
                }
            }

            public static int dialogScreen_DialogId;
            public static AdminBRO.Dialog dialogScreen_DialogData
            {
                get
                {
                    return GameData.GetDialogById(dialogScreen_DialogId);
                }
            }
        }
    }
}
