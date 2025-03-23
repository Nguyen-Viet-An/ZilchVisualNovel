using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

namespace VISUALNOVEL
{
    [System.Serializable]
    public class VN_ConversationDataCompressed
    {
        public List<string> conversation = new List<string>();
        public int startIndex, endIndex;
        public int progress;
        public string fileName;

    }
}