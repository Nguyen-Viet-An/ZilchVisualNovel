using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using UnityEditor;      // For AssetDatabase
using System.IO;        // For Path.ChangeExtension
using VISUALNOVEL;

public class TestDialogueFiles : MonoBehaviour
{

    [SerializeField] private TextAsset fileToRead = null;
    // Start is called before the first frame update
    void Start()
    {
        StartConversation();
    }

    // Update is called once per frame
    void StartConversation()
    {
        
        string fullPath = AssetDatabase.GetAssetPath(fileToRead);
        int resourcesIndex = fullPath.IndexOf("Resources/");
        string relativePath = fullPath.Substring(resourcesIndex + 10);
        string filePath = Path.ChangeExtension(relativePath, null);
        LoadFile(filePath);

    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.DownArrow))
        //     DialogueSystem.instance.dialogueContainer.Hide();
        // else if (Input.GetKeyDown(KeyCode.UpArrow))
        //     DialogueSystem.instance.dialogueContainer.Show();
    }

    public void LoadFile(string filePath)
    {
        List<string> lines = new List<string>();
        TextAsset file = Resources.Load<TextAsset>(filePath);

        try
        {
            lines = FileManager.ReadTextAsset (file);
        }

        catch
        {
            Debug.LogError($"Dialogue file at path 'Resources/{filePath}' does not exist!");
            return;
        }

        DialogueSystem.instance.Say(lines, filePath);
    }


}
