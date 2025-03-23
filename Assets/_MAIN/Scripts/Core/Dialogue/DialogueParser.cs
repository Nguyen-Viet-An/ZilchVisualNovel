using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DIALOGUE
{
    public class DialogueParser
    {
        private const string commandRegexPattern = @"[\w\[\]]*[^\s]\(";
        public static DIALOGUE_LINE Parse(string rawLine)
        {
            Debug.Log($"Parsing Line - '{rawLine}'");

            (string speaker, string dialogue, string commands) = RipContent(rawLine);

            Debug.Log($"Speaker ='{speaker}'\nDialogue = '{dialogue}'\nCommands = '{commands}'");
            
            //We have to inject tags and variables into the speaker and dialogue separately because there are initial checks that have //to be performed.
            //But commands need no checks, so we can inject the variables in them right now.
            commands = TagManager.Inject(commands);

            return new DIALOGUE_LINE(rawLine, speaker, dialogue, commands);           
        }

        private static (string, string, string) RipContent(string rawLine)
        {
            string speaker = "", dialogue = "", commands = "";

            int dialogueStart = -1; //note first quotation mark for start of dialogue section
            int dialogueEnd = -1;   //note last quotation mark for end of dialogue section
            bool isEscaped = false; //note escaped characters such as \" so as to not confuse them for end quotes

            for (int i = 0; i < rawLine.Length; i++)
            {
                char current = rawLine[i];
                if (current == '\\')    // first backslash(\) used as an escape marker so the second backslash registers solely as a backslash and not an escape marker. We escape the escape.
                    isEscaped = !isEscaped;
                else if (current == '"' && !isEscaped)
                {
                    if (dialogueStart == -1)
                        dialogueStart = i;
                    else if (dialogueEnd == -1)
                    {
                        dialogueEnd = i;
                        break;
                    }
                }
                else
                    isEscaped = false;
            }
            
            //Identify command pattern
            Regex commandRegex = new Regex(commandRegexPattern);
            MatchCollection matches = commandRegex.Matches(rawLine);
            int commandStart = -1;

            foreach (Match match in matches)
            {
                if (match.Index < dialogueStart || match.Index > dialogueEnd)
                {
                    commandStart = match.Index;
                    break;
                }
            }

            if (commandStart != -1 && dialogueStart == -1 && dialogueEnd == -1)
                return ("", "", rawLine.Trim());

            //If command is still running, line is either "dialogue" or multi word "command" . Check if dialogue:
            if (dialogueStart != -1 && dialogueEnd != -1 && (commandStart == -1 || commandStart > dialogueEnd)) //if dialogue quote occurs after commandStart then this is valid dialogue:
            {
                speaker = rawLine.Substring(0, dialogueStart).Trim();   //speaker = everything up to dialogueStart and cutting off the rest
                dialogue = rawLine.Substring(dialogueStart + 1, dialogueEnd - dialogueStart - 1).Replace("\\\"", "\"");   //dialogue = everything between dialogueStart and dialogueEnd, changing [\"] to ["]
                if (commandStart != -1)
                    commands = rawLine.Substring(commandStart).Trim();  //commands = everything from commmandStart to end.
            }
            else if (commandStart != -1 && dialogueStart > commandStart)
                commands = rawLine;
            else
                dialogue = rawLine;

            // Debug.Log(rawLine.Substring(dialogueStart + 1, (dialogueEnd - dialogueStart) - 1));
            return (speaker, dialogue, commands);
        }

    }
}