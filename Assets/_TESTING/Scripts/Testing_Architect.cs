using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using DIALOGUE;

namespace TESTING {
    public class Testing_Architect : MonoBehaviour
    {
        DialogueSystem ds;
        TextArchitect architect;
        public TextArchitect.BuildMethod bm = TextArchitect.BuildMethod.instant;
        string[] lines = new string[5] 
        {
            "This is random.",
            "This is lovely.",
            "I hope I can finish this.",
            "Let try to finish chap 3 today.",
            "Hope everything goes smoothly."
        };
        // Start is called before the first frame update
        void Start()
        {
            ds = DialogueSystem.instance;
            architect = new TextArchitect(ds.dialogueContainer.dialogueText);
            architect.buildMethod = TextArchitect.BuildMethod.fade;
        }

        // Update is called once per frame
        void Update()
        {
            if (bm != architect.buildMethod) {
                architect.buildMethod = bm;
                architect.Stop();
            }
            if (Input.GetKeyDown(KeyCode.S)) 
                architect.Stop();
                
            // string longLine = "This is a very long line, I just want to test the hurryup function so hurry up and be long enough so that I don't have to type anymore.This is a very long line, I just want to test the hurryup function so hurry up and be long enough so that I don't have to type anymore.";
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (architect.isBuilding) {
                    if (!architect.hurryUp)
                        architect.hurryUp = true;
                    else 
                       architect.ForceComplete(); 
                } 
                else {
                    // architect.Build(longLine);
                    architect.Build(lines[Random.Range(0, lines.Length)]);
                }   
            }
            else if (Input.GetKeyDown(KeyCode.A)) {
                // architect.Append(longLine);
                architect.Append(lines[Random.Range(0, lines.Length)]);
            }
            
        }
    }
}
