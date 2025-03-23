using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DIALOGUE;

namespace COMMANDS {

    public class CMD_DatabaseExtension_General : CMD_DatabaseExtension
    {
       
        private static string [] PARAM_IMMEDIATE => new string[] {"-i", "-immediate"};
        
        private static string[] PARAM_SPEED => new string[] { "-spd", "-speed" };
        private static readonly string[] PARAM_FILEPATH = new string[] { "-f", "-file", "-filepath" }; 
        private static readonly string[] PARAM_ENQUEUE = new string[] { "-e", "-enqueue" };

        new public static void Extend(CommandDatabase database)
        {
            database.AddCommand("wait", new Func<string, IEnumerator>(Wait));

            //Dialogue Controls system
            database.AddCommand("showui", new Func<String[], IEnumerator>(ShowDialogueSystem)); 
            database.AddCommand("hideui", new Func<String[], IEnumerator>(HideDialogueSystem));
            
            //Dialogue Controls Box
            database.AddCommand("showdb", new Func<String[], IEnumerator>(ShowDialogueBox)); 
            database.AddCommand("hidedb", new Func<String[], IEnumerator>(HideDialogueBox));

            database.AddCommand("load", new Action<string[]>(LoadNewDialogueFile));
        }

        private static void LoadNewDialogueFile(string[] data)
        {
            string fileName = string.Empty;
            bool enqueue = false;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_FILEPATH, out fileName);
            parameters.TryGetValue(PARAM_ENQUEUE, out enqueue, defaultValue: false);

            string filePath = FilePaths.GetPathToResource(FilePaths.resources_dialogueFiles, fileName);
            TextAsset file = Resources.Load<TextAsset>(filePath);

            if (file == null)
            {
                Debug.LogWarning($"File '{filePath}' could not be loaded from dialogue files. Please ensure it exists within the '{FilePaths.resources_dialogueFiles}' resources folder.");
                return;
            }

            List<string> lines = FileManager.ReadTextAsset(file, includeBlankLines: true); 
            Conversation newConversation = new Conversation(lines);

            if (enqueue)
            {
                Debug.Log("Enqueue true");
                DialogueSystem.instance.conversationManager.Enqueue(newConversation);
            }
            else
            {   
                Debug.Log("Enqueue false");
                DialogueSystem.instance.conversationManager.StartConversation(newConversation);
            }
                
        }
        
        private static IEnumerator Wait(string data)
        {
            if (float.TryParse(data, out float time))
            {
                yield return new WaitForSeconds(time);
            }
        }
        
        private static IEnumerator ShowDialogueBox(string[] data)
        {
            float speed;
            bool immediate;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f); 
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

            yield return DialogueSystem.instance.dialogueContainer.Show(speed, immediate);
        }


        private static IEnumerator HideDialogueBox(string[] data)
        {
            float speed;
            bool immediate;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f); 
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

            yield return DialogueSystem.instance.dialogueContainer.Hide(speed, immediate);
        }

        private static IEnumerator ShowDialogueSystem(string[] data)
        {
            float speed;
            bool immediate;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f); 
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

            yield return DialogueSystem.instance.Show(speed, immediate);
        }


        private static IEnumerator HideDialogueSystem(string[] data)
        {
            float speed;
            bool immediate;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_SPEED, out speed, defaultValue: 1f); 
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);

            yield return DialogueSystem.instance.Hide(speed, immediate);
        }

    }
 
}
