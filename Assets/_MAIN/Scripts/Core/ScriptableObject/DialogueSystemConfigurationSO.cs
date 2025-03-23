using CHARACTERS;
using TMPro;
using UnityEngine;

namespace DIALOGUE {
    [CreateAssetMenu(fileName ="Dialogue Configuration Asset", menuName = "Dialogue System/Dialogue Configuration Asset")]
    public class DialogueSystemConfigurationSO : ScriptableObject
    {

        public const float DEFAULT_FONTSIZE_DIALOGUE = 60; 
        public const float DEFAULT_FONTSIZE_NAME = 30;
        public CharacterConfigSO characterConfigurationAsset;
        public Color defaultTextColor = Color.white;
        public TMP_FontAsset defaultFont;
        public float dialogueFontScale = 2f;
        public float defaultDialogueFontSize = DEFAULT_FONTSIZE_DIALOGUE;
        public float defaultNameFontSize = DEFAULT_FONTSIZE_NAME;
    }
}
