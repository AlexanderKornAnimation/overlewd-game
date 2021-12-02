using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    namespace FTUE
    {

        public static class GameData
        {
            public static AdminBRO.Dialog GetDialogById(int id) 
            {
                return dialogs.Find(d => d.id == id);
            }
            public static List<AdminBRO.Dialog> dialogs = new List<AdminBRO.Dialog>();

            public static void Initialization()
            {
                dialogs.Add(new AdminBRO.Dialog {
                    id = 0,
                    replicas = new List<AdminBRO.DialogReplica>
                    {
                        new AdminBRO.DialogReplica { characterName = "PersName1", message = "Message 1" },
                        new AdminBRO.DialogReplica { characterName = "PersName2", message = "Message 2" },
                        new AdminBRO.DialogReplica { characterName = "PersName1", message = "Message 3" },
                        new AdminBRO.DialogReplica { characterName = "PersName2", message = "Message 4" },
                    }
                });

                dialogs.Add(new AdminBRO.Dialog
                {
                    id = 1,
                    replicas = new List<AdminBRO.DialogReplica>
                    {
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Overlord, 
                            message = "Message Owerlord",
                            characterPosition = AdminBRO.DialogCharacterPosition.Left
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Ulvi,
                            message = "Message Ulvi",
                            characterPosition = AdminBRO.DialogCharacterPosition.Right
                        },
                        new AdminBRO.DialogReplica {
                            characterName = AdminBRO.DialogCharacterName.Faye,
                            message = "Message Faye",
                            characterPosition = AdminBRO.DialogCharacterPosition.Middle
                        },
                    }
                });
            }
        }
    }
}
