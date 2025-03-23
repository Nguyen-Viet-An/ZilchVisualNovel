using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;

public class InputPanelTesting : MonoBehaviour
{
    public InputPanel inputPanel;
    //Start is called before the first frame update

    void Start()
    {
        StartCoroutine(Running());
    }

    IEnumerator Running()
    {
        Character Billy = CharacterManager.instance.CreateCharacter("Billy", revealAfterCreation: true);
        yield return Billy.Say("Hi! What's your name?");

        inputPanel.Show("What Is Your Name?");
        while (inputPanel.isWaitingOnUserInput) 
            yield return null;
        string characterName = inputPanel.lastInput;

        yield return Billy.Say($"It's very nice to meet you, {characterName}!");

    }
}
