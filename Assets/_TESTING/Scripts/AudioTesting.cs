using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using DIALOGUE;
public class AudioTesting : MonoBehaviour
{

// Start is called before the first frame update

    void Start()
    {
        StartCoroutine(Running());
    }

    Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);
    IEnumerator Running()
    {
        Character_Sprite Billy = CreateCharacter("Billy") as Character_Sprite; 
        

        Billy.Show();

        // yield return DialogueSystem.instance.Say("Narrator", "Can we see your ship?");
        // GraphicPanelManager.instance.GetPanel ("background").GetLayer(0, true).SetTexture("Graphics/BG Images/5"); 
        // AudioManager.instance.PlayTrack("Audio/Music/Calm", volumeCap: 0.5f);
        // AudioManager.instance.PlayVoice("Audio/Voices/exclamation");
        // Billy.SetSprite(Billy.GetSprite("Billy Happy"), 0);
        // Billy.MoveToPosition(new Vector2(0.7f, 0), speed: 0.5f); 
        // yield return Billy.Say("Yes, of course!");

        
        // yield return Billy.Say("Let me show you the engine room.");
        // GraphicPanelManager.instance.GetPanel ("background").GetLayer(0, true).SetTexture("Graphics/BG Images/EngineRoom"); 
        // AudioManager.instance.PlayTrack("Audio/Music/Comedy", volumeCap: 0.8f);

        Character Me = CreateCharacter("Me");
        GraphicPanelManager.instance.GetPanel("background").GetLayer(0, true).SetTexture("Graphics/BG Images/villagenight");
        AudioManager.instance.PlayTrack("Audio/Ambience/RainyMood", 0); 
        AudioManager.instance.PlayTrack("Audio/Music/Calm", 1, pitch: 0.7f);
        yield return Billy.Say("We can have multiple channels for playing ambience as well as music!");
        AudioManager.instance.StopTrack(1);


        // yield return new WaitForSeconds(0.5f);

        // AudioManager.instance.PlaySoundEffect("Audio/SFX/thunder_strong_01");

        // yield return new WaitForSeconds (1f);

        // Billy.Animate("Hop");
        // Billy.TransitionSprite(Billy.GetSprite("Billy Angry")); 


    }
}
