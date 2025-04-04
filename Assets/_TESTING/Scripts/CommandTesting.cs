using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using COMMANDS;
namespace TESTING {
    public class CommandTesting : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            // StartCoroutine(Running());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                CommandManager.instance.Execute("moveCharDemo", "left");
            else if (Input.GetKeyDown(KeyCode. RightArrow))
                CommandManager.instance.Execute("moveCharDemo", "right");
        }

        IEnumerator Running()
        {
            yield return CommandManager.instance.Execute("print");
            yield return CommandManager.instance.Execute("print_1p", "Hello world");
            yield return CommandManager.instance.Execute("print_mp", "Line 1", "Line 2", "Line 3");

            yield return CommandManager.instance.Execute("lambda");
            yield return CommandManager.instance.Execute("lambda_1p", "Hello Lambda");
            yield return CommandManager.instance.Execute("lambda_mp", "Lambda 1", "Lambda 2", "Lambda 3");

            yield return CommandManager.instance.Execute("process");
            yield return CommandManager.instance.Execute("process_1p", "3");
            yield return CommandManager.instance.Execute("process_mp", "Process Line 1", "Process Line 2", "Process Line 3"); 
        }
    }
}
