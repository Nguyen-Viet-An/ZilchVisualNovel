using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DIALOGUE;
using UnityEngine;
using UnityEngine.UI;

namespace CHARACTERS {
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager instance { get; private set; }
        public  Character[] allCharacters => characters.Values.ToArray();
        private Dictionary<string, Character> characters = new Dictionary<string, Character>();
        private CharacterConfigSO config => DialogueSystem.instance.config.characterConfigurationAsset;
        public const string CHARACTER_CASTING_ID = " as ";
        private const string CHARACTER_NAME_ID = "<charname>";
        public string characterRootPathFormat => $"Characters/{CHARACTER_NAME_ID}";
        public string characterPrefabNameFormat => $"Character - [{CHARACTER_NAME_ID}]";
        public string characterPrefabPathFormat => $"{characterRootPathFormat}/{characterPrefabNameFormat}";
        [SerializeField] private RectTransform _characterpanel = null;
        public RectTransform characterPanel => _characterpanel;

        private void Awake() {
            instance = this;
        }

        public CharacterConfigData GetCharacterConfig(string characterName, bool getOriginal = false) 
        { 
            if (!getOriginal)
            {
                Character character = GetCharacter(characterName);
                if (character != null)
                    return character.config;
            }

            return config.GetConfig(characterName);
        }

        public Character GetCharacter(string characterName, bool createIfDoesNotExist = false)
        {
            if (characters. ContainsKey(characterName.ToLower()))
                return characters[characterName.ToLower()];
            else if (createIfDoesNotExist)
                return CreateCharacter(characterName);
            return null;
        }
        public bool HasCharacter(string characterName) => characters.ContainsKey(characterName.ToLower());
        
        public Character CreateCharacter(string characterName, bool revealAfterCreation=false)
        {
            if (characters.ContainsKey(characterName.ToLower()))
            {
                Debug.LogWarning($"A Character called '{characterName}' already exists. Did not create the character."); 
                return null;
            }

            CHARACTER_INFO info = GetCharacterInfo(characterName);
            Character character = CreateCharacterFromInfo(info);

            if (info.castingName != info.name)
                character.castingName = info.castingName;

            characters.Add(characterName.ToLower(), character);

            if (revealAfterCreation)
                character.Show();
                
            return character;
        }
        private CHARACTER_INFO GetCharacterInfo(string characterName)
        {
            CHARACTER_INFO result = new CHARACTER_INFO();
            string[] nameData = characterName.Split(CHARACTER_CASTING_ID, System.StringSplitOptions.RemoveEmptyEntries); 
            result.name = nameData[0];
            result.castingName = nameData.Length > 1 ? nameData[1] : result.name;

            result.config = config.GetConfig(result.castingName);

            result.prefab = GetPrefabForCharacter(result.castingName);

            result.rootCharacterFolder = FormatCharacterPath(characterRootPathFormat, result.castingName);

            return result;
        }
        
        private GameObject GetPrefabForCharacter(string characterName)
        {
            string prefabPath = FormatCharacterPath(characterPrefabPathFormat, characterName); 
            return Resources.Load<GameObject>(prefabPath);
        }
        public string FormatCharacterPath(string path, string characterName) => path.Replace(CHARACTER_NAME_ID, characterName);
        private Character CreateCharacterFromInfo(CHARACTER_INFO info)
        {
            CharacterConfigData config = info.config;
            switch(config.characterType)
            {
                case Character.CharacterType.Text: 
                    return new Character_Text(info.name, config);
                case Character.CharacterType.Sprite:
                case Character.CharacterType.SpriteSheet:
                    return new Character_Sprite(info.name, config, info.prefab, info.rootCharacterFolder);
                default:
                    return null;
            }    
        }
        private class CHARACTER_INFO
        {
            public string name = "";
            public string castingName = "";
            public CharacterConfigData config = null;
            public string rootCharacterFolder = "";
            public GameObject prefab = null;
        }
    }
}
