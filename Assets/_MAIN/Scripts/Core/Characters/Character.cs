using DIALOGUE;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEditor;
using Unity.Collections;
using System.Security.Cryptography;


namespace CHARACTERS {
// Start is called before the first frame updateEnamespace CHARACTERS
    public abstract class Character
    {
        public const bool ENABLE_ON_START = false;
        private const float UNHIGHTLIGHTED_DARKEN_STRENGTH = 0.65f;
        public const bool DEFAULT_ORIENTATION_IS_FACING_RIGHT = true;
        public const string ANIMATION_REFRESH_TRIGGER = "Refresh";
        public string name = "";
        public string displayName = "";
        public string castingName = "";
        public RectTransform root = null;
        public CharacterConfigData config;
        public Animator animator;
        public Color color { get; protected set; } = Color.white;
        protected Color displayColor => highlighted ? highlightedColor : unhighlightedColor;
        protected Color highlightedColor => color;
        protected Color unhighlightedColor => new Color(color.r * UNHIGHTLIGHTED_DARKEN_STRENGTH, color.b * UNHIGHTLIGHTED_DARKEN_STRENGTH, color.g * UNHIGHTLIGHTED_DARKEN_STRENGTH, color.a);
        public bool highlighted { get; protected set;} = true;
        protected bool facingRight = DEFAULT_ORIENTATION_IS_FACING_RIGHT;
        public Vector2 targetPosition { get; private set; }
        protected CharacterManager characterManager => CharacterManager.instance;
        public DialogueSystem dialogueSystem => DialogueSystem.instance;

        // Coroutines
        protected Coroutine co_revealing, co_hiding;
        protected Coroutine co_moving;
        protected Coroutine co_changingColor;
        protected Coroutine co_highlighting;
        protected Coroutine co_flipping;
        public bool isRevealing => co_revealing != null;
        public bool isHiding => co_hiding != null; 
        public bool isMoving => co_moving != null;
        public bool isChangingColor => co_changingColor != null;
        public bool isHighlighting => (highlighted && co_highlighting != null);
        public bool isUnHighlighting => (!highlighted && co_highlighting != null);
        public virtual bool isVisible {get; set; }
        public bool isFacingRight => facingRight;
        public bool isFacingLeft => !facingRight;
        public bool isFlipping => co_flipping != null;

        public Character(string name, CharacterConfigData config, GameObject prefab) {
            this.name = name;
            displayName = name;
            this.config = config; 
            if (prefab != null) {
                GameObject ob = Object.Instantiate(prefab, characterManager.characterPanel);
                ob.name = characterManager.FormatCharacterPath(characterManager.characterPrefabNameFormat, name);
                ob.SetActive(true);
                root = ob.GetComponent<RectTransform>();
                animator = root.GetComponentInChildren<Animator>();
            }
        }
        private List<string> FormatDialogue(List<string> dialogue)
        {
            List<string> formattedDialogue = new List<string>();

            foreach (string line in dialogue)
            {
                formattedDialogue.Add($"{displayName} \"{line}\"");
            }
            
            return formattedDialogue;
        }

        public Coroutine Say(string dialogue) => Say(new List<string> { dialogue });
        public Coroutine Say(List<string> dialogue)
        {
            dialogueSystem.ShowSpeakerName(displayName);
            UpdateTextCustomizationsOnScreen();
            dialogue = FormatDialogue(dialogue);
            return dialogueSystem.Say(dialogue);
        }

            
        public void SetNameFont (TMP_FontAsset font) => config.nameFont = font;
        public void SetDialogueFont (TMP_FontAsset font) => config.dialogueFont = font;
        public void SetNameColor(Color color) => config.nameColor = color;
        public void SetDialogueColor(Color color) => config.dialogueColor = color;
        public void ResetConfigurationData() => config = CharacterManager.instance.GetCharacterConfig(name, getOriginal: true);
        public void UpdateTextCustomizationsOnScreen() => dialogueSystem.ApplySpeakerDataToDialogueContainer(config);

        public virtual Coroutine Show(float speedMultiplier = 1f)
        {
            if (isRevealing)
                characterManager.StopCoroutine(co_revealing);

            if (isHiding)
                characterManager.StopCoroutine(co_hiding);

            co_revealing = characterManager.StartCoroutine(ShowingOrHiding(true, speedMultiplier));
            return co_revealing;
        }

        public virtual Coroutine Hide(float speedMultiplier = 1f)
        {
            if (isHiding)
                characterManager.StopCoroutine(co_hiding);

            if (isRevealing)
                characterManager.StopCoroutine (co_revealing);

            co_hiding = characterManager.StartCoroutine(ShowingOrHiding(false, speedMultiplier));
            return co_hiding;
        }

        public virtual IEnumerator ShowingOrHiding(bool show, float speedMultiplier = 1f)
        {
            Debug.Log("Show/Hide cannot be called from a base character type. ");
            yield return null;
        }

        
        public virtual void SetPosition(Vector2 position)
        {
            if (root == null)
                return;
            (Vector2 minAnchorTarget, Vector2 maxAnchorTarget) = ConvertUITargetPositionToRelativeCharacterAnchorTargets(position); 
            root.anchorMin = minAnchorTarget;
            root.anchorMax = maxAnchorTarget;

            targetPosition = position;
        }

        public virtual Coroutine MoveToPosition(Vector2 position, float speed = 2f, bool smooth = false) 
        {
            if (root == null)
                return null;

            if (isMoving)
                characterManager.StopCoroutine(co_moving);

            co_moving = characterManager.StartCoroutine(MovingToPosition(position, speed, smooth));

            targetPosition = position;
            return co_moving;

        }

        private IEnumerator MovingToPosition(Vector2 position, float speed, bool smooth)
        {
            (Vector2 minAnchorTarget, Vector2 maxAnchorTarget) = ConvertUITargetPositionToRelativeCharacterAnchorTargets(position); 
            Vector2 padding = root.anchorMax - root.anchorMin;
            while (root.anchorMin != minAnchorTarget || root.anchorMax != maxAnchorTarget)
            {
                root.anchorMin = smooth ?
                Vector2.Lerp(root.anchorMin, minAnchorTarget, speed * Time.deltaTime)
                : Vector2.MoveTowards (root.anchorMin, minAnchorTarget, speed * Time.deltaTime * 0.35f);

                root.anchorMax = root.anchorMin + padding;

                if (smooth && Vector2. Distance(root.anchorMin, minAnchorTarget) <= 0.001f) 
                {
                    root.anchorMin = minAnchorTarget;
                    root.anchorMax = maxAnchorTarget; 
                    break;
                }

                yield return null;
            }

            Debug.Log("Done moving");
            co_moving = null;
        }
        protected (Vector2, Vector2) ConvertUITargetPositionToRelativeCharacterAnchorTargets(Vector2 position)
        {
            Vector2 padding = root.anchorMax - root.anchorMin;
            float maxX = 1f - padding.x;
            float maxY = 1f - padding.y;
            Vector2 minAnchorTarget = new Vector2(maxX * position.x, maxY * position.y); 
            Vector2 maxAnchorTarget = minAnchorTarget + padding;
            return (minAnchorTarget, maxAnchorTarget);
        }

        public virtual void SetColor(Color color)
        {
            this.color = color;
        }

        public Coroutine TransitionColor(Color color, float speed = 1f)
        {
            this.color = color;
            if (isChangingColor)
                characterManager.StopCoroutine(co_changingColor);

            co_changingColor = characterManager.StartCoroutine(ChangingColor(displayColor, speed));
            return co_changingColor;
        }
        public virtual IEnumerator ChangingColor(Color color, float speed)
        {
            Debug.Log("Color changing is not applicable on this character type!"); 
            yield return null;
        }

        
        public Coroutine Highlight(float speed = 1f, bool immediate = false)
        {
            if (isHighlighting || isUnHighlighting)
                characterManager.StopCoroutine(co_highlighting);

            highlighted = true;

            co_highlighting = characterManager.StartCoroutine(Highlighting(speed, immediate));

            return co_highlighting;
        }
        
        public Coroutine UnHighlight(float speed = 1f, bool immediate = false)
        {
            if (isHighlighting || isUnHighlighting)
                characterManager.StopCoroutine(co_highlighting);

            highlighted = false;

            co_highlighting = characterManager.StartCoroutine(Highlighting(speed, immediate));

            return co_highlighting;
        }

        public virtual IEnumerator Highlighting(float speedMultiplier, bool immediate)
        {
            Debug.Log("Highlighting is not available on this character type!"); 
            yield return null;
        }

        public Coroutine Flip(float speed = 1, bool immediate = false) {
            if (isFacingRight)
                return FaceLeft(speed, immediate);
            else
                return FaceRight(speed, immediate);  
        }

        public Coroutine FaceLeft(float speed = 1, bool immediate = false)
        {
            if (isFlipping)
                characterManager.StopCoroutine(co_flipping);

            facingRight = false;
            co_flipping = characterManager.StartCoroutine(FaceDirection(facingRight, speed, immediate));
            return co_flipping;
        }

        public Coroutine FaceRight(float speed = 1, bool immediate = false)
        {
            if (isFlipping)
                characterManager.StopCoroutine(co_flipping);

            facingRight = true;
            co_flipping = characterManager.StartCoroutine(FaceDirection(facingRight, speed, immediate));
            return co_flipping;
        }

        public virtual IEnumerator FaceDirection(bool faceRight, float speedMultiplier, bool immediate)
        {
            Debug.Log("Cannot flip a character of this type!");
            yield return null;
        }

        public void Animate(string animation)
        {
            animator.SetTrigger(animation);
        }
        public void Animate(string animation, bool state)
        {
            animator.SetBool(animation, state); 
            animator.SetTrigger(ANIMATION_REFRESH_TRIGGER);
        }

        public virtual void OnReceiveCastingExpression(int layer, string expression) {
            return;
        }
                
        public enum CharacterType
        {
            Text, 
            Sprite,
            SpriteSheet,
            Live2D,
            Model3D
        }
    }
}

