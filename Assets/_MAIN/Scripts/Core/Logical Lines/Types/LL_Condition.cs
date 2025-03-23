using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static DIALOGUE.LogicalLines.LogicalLineUtils.Encapsulation;
using static DIALOGUE.LogicalLines.LogicalLineUtils.Conditions;

namespace DIALOGUE.LogicalLines
{

    public class LL_Condition : ILogicalLine
    {

        public string keyword => "if";
        private const string ELSE = "else";
        private readonly string[] CONTAINERS = new string[] { "(", ")" };

        public IEnumerator Execute(DIALOGUE_LINE line)
        {
            string rawCondition = ExtractCondition(line.rawData.Trim()); 
            bool conditionResult = EvaluateCondition(rawCondition);
            Debug.Log($"conditionResult: {conditionResult}");

            Conversation currentConversation = DialogueSystem.instance.conversationManager.conversation;
            int currentProgress = DialogueSystem.instance.conversationManager.conversationProgress;

            EncapsulatedData ifData = RipEncapsulationData(currentConversation, currentProgress, false, parentStartingIndex: currentConversation.fileStartIndex); 
            Debug.Log($"ifData: {ifData}");
            EncapsulatedData elseData = new EncapsulatedData();

            if (ifData.endingIndex + 1 < currentConversation.Count)
            {
                string nextLine = currentConversation.GetLines()[ifData.endingIndex + 1].Trim(); 
                Debug.Log($"nextLine: {nextLine}");
                if (nextLine == ELSE)
                {
                    elseData = RipEncapsulationData(currentConversation, ifData.endingIndex + 1, false, parentStartingIndex: currentConversation.fileStartIndex); 
                    Debug.Log($"elseData: {elseData}");
                }
            }

            currentConversation.SetProgress(elseData.isNull ? ifData.endingIndex : elseData.endingIndex);
            EncapsulatedData selData = conditionResult ? ifData : elseData;
            Debug.Log($"selData: {selData}");
            if (!selData.isNull && selData.lines.Count > 0)
            {
                //Remove the header and encapsulator lines from the conversation indexes 
                selData.startingIndex += 2;
                selData.endingIndex -= 1;
                
                Conversation newConversation = new Conversation(selData.lines, file: currentConversation.file, fileStartIndex: selData.startingIndex, fileEndIndex: selData.endingIndex);
                DialogueSystem.instance.conversationManager.EnqueuePriority(newConversation);
            }

            yield return null;
        }

        public bool Matches (DIALOGUE_LINE line)
        {
            return line.rawData.Trim().StartsWith(keyword);
        }

        private string ExtractCondition(string line)
        {
            int startIndex = line.IndexOf(CONTAINERS[0]) + 1; 
            int endIndex = line.IndexOf(CONTAINERS[1]);
            Debug.Log($"Extracted line: {line.Substring(startIndex, endIndex - startIndex).Trim()}");
            return line.Substring(startIndex, endIndex - startIndex).Trim();
        }
    }
}