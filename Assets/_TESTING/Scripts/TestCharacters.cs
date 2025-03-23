using System.Collections;
using System.Collections.Generic;
using CHARACTERS;
using TMPro;
using UnityEngine;

public class TestCharacters : MonoBehaviour
{
    public TMP_FontAsset tempFont;
    private Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);
    // Start is called before the first frame update
    void Start()
    {
        // Character Walter = CharacterManager.instance.CreateCharacter("Walter");
        // Character Billy = CharacterManager.instance.CreateCharacter("Billy");
        // Character Billy2 = CharacterManager.instance.CreateCharacter("Walter");
        // Character Adam = CharacterManager.instance.CreateCharacter("Adam");
        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        Character_Sprite Billy = CreateCharacter("Billy") as Character_Sprite; 
        Character_Sprite Walter = CreateCharacter("Walter") as Character_Sprite;
        Billy.SetPosition(Vector2.zero);
        Walter.SetPosition(new Vector2(1, 0));
        yield return Billy.Flip(0.3f);
        yield return Walter.FaceLeft(immediate:true);
        yield return Billy.FaceRight(immediate:true);
        Billy.Animate("Hop");
        Walter.Animate("Shiver", true);
        Walter.UnHighlight();
        yield return Billy.Say("I want to say something.");
        Billy.UnHighlight();
        Walter.Highlight();
        Walter.Animate("Shiver", false);
        yield return Walter.Say( "But I want to say something too! {c}Can I go first?");
        Billy. Highlight();
        Walter. UnHighlight();
        yield return Billy.Say("Sure, {a} be my guest.");
        Walter.Highlight();
        Billy. UnHighlight();
        Walter.TransitionSprite(Walter.GetSprite("Happy 1"), layer: 1);
        yield return Walter.Say("Yay!");
        yield return null;
        // Character_Sprite Walter = CreateCharacter("Walter") as Character_Sprite; 
        // Walter.isVisible = false;
        // Character_Sprite guard3 = CreateCharacter("Guard3 as Billy") as Character_Sprite;

        // guard1.SetPosition(Vector2.zero);
        // guard2.SetPosition(new Vector2(0.5f, 0.5f));
        // guard3.SetPosition(Vector2.one);

        // guard1.Show();
        // guard2.Show();
        // guard3.Show();

        // Sprite faceSprite = Billy.GetSprite("Billy Sad");
        // Billy.TransitionSprite(faceSprite);

        // faceSprite = guard1.GetSprite("Billy Angry");
        
        // Billy.MoveToPosition(Vector2.zero);
        // yield return Walter.Show();

        // yield return new WaitForSeconds(1f);
        // Billy.TransitionSprite(Billy.GetSprite("Billy Happy"));
        yield return null;


        // guard1.MoveToPosition(Vector2.one, smooth: true);
        // guard1.MoveToPosition(Vector2.zero, smooth: true);

        // guard1.SetDialogueFont(tempFont);
        // guard1.SetNameFont (tempFont);
        // guard2.SetDialogueColor(Color.cyan); guard3.SetNameColor(Color.red);

        // yield return guard1. Say("I want to say something important.");
        // yield return guard2. Say("Hold your peace.");
        // yield return guard3. Say("Let him speak...|");

    
        // Character Walter = CharacterManager.instance.CreateCharacter("Walter"); 
        // Character Billy = CharacterManager.instance.CreateCharacter("Billy"); 
        // Character Sarah = CharacterManager.instance.CreateCharacter("Sarah");

        // List<string> lines = new List<string>()
        // {
        //     "Hi, there!",
        //     "My name is Walter.",
        //     "What's your name?",
        //     "Oh, {wa 1} that's very nice."
        // };

        // yield return Walter.Say(lines);

        // Walter.SetNameColor(Color.red);
        // Walter.SetDialogueColor(Color.green);
        // Walter.SetNameFont(tempFont);
        // Walter.SetDialogueFont(tempFont);
        // yield return Walter.Say(lines);

        // Walter.ResetConfigurationData();
        // yield return Walter.Say(lines);

        // lines = new List<string>()
        // {
        //     "I am Billy.",
        //     "More Lines{c}Here."
        // };
        
        // yield return Billy.Say(lines);
        // yield return Sarah.Say("This is a line that I want to say. {a} It is a simple line.");
        // Debug.Log("Finished");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
