using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using DIALOGUE;

public class GraphicLayerTesting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Running());
    }

    IEnumerator Running() {
        // GraphicPanelManager.instance.GetPanel("Background").GetLayer(0, true);
        GraphicPanel panel = GraphicPanelManager.instance.GetPanel("Background");
        // GraphicLayer layer = panel.GetLayer(0, true);

        // yield return new WaitForSeconds(1);

        // Texture blendTex = Resources.Load<Texture>("Graphics/Transition Effects/hurricane");
        // layer.SetTexture("Graphics/BG Images/2", blendingTexture: blendTex);

        // yield return new WaitForSeconds(1);

        // // layer.currentGraphic.renderer.material.SetColor("_Color", Color.red);
        // layer.SetVideo("Graphics/BG Videos/Fantasy Landscape", blendingTexture: blendTex);

        // yield return new WaitForSeconds(3);

        // layer.currentGraphic.FadeOut();

        // yield return new WaitForSeconds(3);
        // Debug.Log(layer.currentGraphic);

        
        GraphicLayer layer0 = panel.GetLayer(0, true); 
        GraphicLayer layer1 = panel.GetLayer(1, true);

        layer0.SetVideo ("Graphics/BG Videos/Nebula");
        layer1.SetTexture("Graphics/BG Images/Spaceshipinterior");

        yield return new WaitForSeconds(2);

        GraphicPanel cinematic = GraphicPanelManager.instance.GetPanel("Cinematic"); 
        GraphicLayer cinLayer = cinematic.GetLayer(0, true);

        Character Billy = CharacterManager.instance.CreateCharacter("Billy", true);

        yield return Billy.Say("Let's take a look at a picture on the cinematic layer."); 

        cinLayer.SetTexture("Graphics/Gallery/pup");
        yield return DialogueSystem.instance.Say("Narrator", "We truly don't deserve dogs");
        cinLayer.Clear();
        
        yield return new WaitForSeconds(1);
        panel.Clear();
    }

}
