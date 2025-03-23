using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace COMMANDS
{
    
    public class CMD_Database_VisualNovel : CMD_DatabaseExtension
    {
            new public static void Extend(CommandDatabase database)
            {
                // Variable Assignment
                database.AddCommand("setplayername", new Action<string>(SetPlayerNameVariable));
            }

            
            private static void SetPlayerNameVariable(string data)
            {
                VISUALNOVEL.VNGameSave.activeFile.playerName = data;
            }

            

    }
}

