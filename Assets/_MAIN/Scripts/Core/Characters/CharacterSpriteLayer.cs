using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace CHARACTERS {
    public class CharacterSpriteLayer 
    {
        private CharacterManager characterManager => CharacterManager.instance;
        private const float DEFAULT_TRANSITION_SPEED = 3f; 
        private float transitionSpeedMultiplier = 1f;
        public int layer { get; private set; } = 0;
        public Image renderer { get; private set; } = null;
        public CanvasGroup rendererCG => renderer.GetComponent<CanvasGroup>();
        private List<CanvasGroup> oldRenderers = new List<CanvasGroup>();
        private Coroutine co_transitioningLayer = null;
        private Coroutine co_levelingAlpha = null;
        private Coroutine co_changingColor = null;
        private Coroutine co_flipping = null;
        private bool isFacingRight = Character.DEFAULT_ORIENTATION_IS_FACING_RIGHT;
        public bool isTransitioningLayer => co_transitioningLayer != null;
        public bool isLevelingAlpha => co_levelingAlpha != null;
        public bool isChangingColor => co_changingColor != null;
        public bool isFlipping => co_flipping != null;
        public CharacterSpriteLayer(Image defaultRenderer, int layer = 0)
        {
            renderer = defaultRenderer;
            this.layer = layer;
        }
        public void SetSprite(Sprite sprite)
        {
            renderer.sprite = sprite;
        }

        public Coroutine TransitionSprite(Sprite sprite, float speed = 1)
        {
            if (sprite == renderer.sprite)
                return null;

            if (isTransitioningLayer)
                characterManager.StopCoroutine(co_transitioningLayer);

            co_transitioningLayer = characterManager.StartCoroutine(TransitioningSprite(sprite, speed));

            return co_transitioningLayer;
        }

        private IEnumerator TransitioningSprite(Sprite sprite, float speedMultiplier)
        {
            transitionSpeedMultiplier = speedMultiplier;
            Image newRenderer = CreateRenderer(renderer.transform.parent);
            newRenderer.sprite = sprite;
            yield return TryStartLevelingAlphas();
            co_transitioningLayer = null;
        }

        private Image CreateRenderer(Transform parent)
        {
            Image newRenderer = Object.Instantiate(renderer, parent); 
            oldRenderers.Add(rendererCG);

            newRenderer.name = renderer.name; 
            renderer = newRenderer; 
            renderer.gameObject.SetActive(true); 
            rendererCG.alpha = 0;

            return newRenderer;
        }

        private Coroutine TryStartLevelingAlphas()
        {
            if (isLevelingAlpha)
                characterManager.StopCoroutine(co_levelingAlpha);

            co_levelingAlpha = characterManager.StartCoroutine(RunAlphaLeveling());

            return co_levelingAlpha;
        }
        private IEnumerator RunAlphaLeveling()
        {
            while (rendererCG.alpha < 1 || oldRenderers.Any(oldCG => oldCG.alpha > 0))
            {
                float speed = DEFAULT_TRANSITION_SPEED * transitionSpeedMultiplier * Time.deltaTime;
                rendererCG.alpha = Mathf.MoveTowards(rendererCG.alpha, 1, speed);
                for (int i = oldRenderers.Count - 1; i >= 0; i--)
                {
                    CanvasGroup oldCG = oldRenderers[i];
                    oldCG.alpha = Mathf.MoveTowards(oldCG.alpha, 0, speed);
                    if (oldCG.alpha <= 0)
                    {
                        oldRenderers.RemoveAt(i); 
                        Object.Destroy(oldCG.gameObject);
                        
                    }
                    
                }
                yield return null;
                        
            }
            co_levelingAlpha = null;
        }

        public void SetColor(Color color)
        {
            renderer.color = color;
            foreach (CanvasGroup oldCG in oldRenderers)
            {
                oldCG.GetComponent<Image>().color = color;
            }
        }

        public Coroutine TransitionColor(Color color, float speed)
        {
            if (isChangingColor)
                characterManager. StopCoroutine (co_changingColor);
            co_changingColor = characterManager.StartCoroutine(ChangingColor(color, speed));
            return co_changingColor;
        }

        public void StopChangingColor()
        {
            if (!isChangingColor)
                return;
            characterManager.StopCoroutine(co_changingColor);
            co_changingColor = null;
        }
        
        private IEnumerator ChangingColor(Color color, float speedMultiplier)
        {
            Color oldColor = renderer.color;
            List<Image> oldImages = new List<Image>();
            foreach (var oldCG in oldRenderers)
            {
                oldImages.Add(oldCG.GetComponent<Image>());
            }
            float colorPercent = 0;
            while (colorPercent < 1)
            {
                colorPercent += DEFAULT_TRANSITION_SPEED * speedMultiplier * Time.deltaTime;
                renderer.color = Color.Lerp(oldColor, color, colorPercent);

                for (int i = oldImages.Count - 1; i >= 0; i--)
                {
                    Image image = oldImages[i];
                    if (image != null)
                        image.color = renderer.color;
                    else
                        oldImages. RemoveAt(i);
                }
                
                yield return null;
            }
            co_changingColor = null;
        }

        public Coroutine Flip(float speed = 1, bool immediate = false) {
            if (isFacingRight)
                return FaceLeft(speed, immediate);
            else
                return FaceRight(speed, immediate);  
        }

        public Coroutine FaceLeft(float speed = 1, bool immediate = false) {
            if (isFlipping)
                characterManager. StopCoroutine(co_flipping);
            isFacingRight = false;
            co_flipping = characterManager. StartCoroutine (FaceDirection(isFacingRight, speed, immediate));
            return co_flipping;
        }

        public Coroutine FaceRight(float speed = 1, bool immediate = false)
        {
            if (isFlipping)
                characterManager.StopCoroutine(co_flipping);

            isFacingRight = true;
            co_flipping = characterManager.StartCoroutine(FaceDirection(isFacingRight, speed, immediate));
            return co_flipping;
        }

        
        private IEnumerator FaceDirection(bool faceRight, float speedMultiplier, bool immediate) 
        {
            float xScale = faceRight ? 1 : -1;
            Vector3 newScale = new Vector3(xScale, 1, 1);
            if (!immediate)
            {
                Image newRenderer = CreateRenderer(renderer.transform.parent);
                newRenderer.transform.localScale = newScale;
                transitionSpeedMultiplier = speedMultiplier; 
                TryStartLevelingAlphas();

                while (isLevelingAlpha) 
                    yield return null;
            }
            else
            {
                renderer.transform.localScale = newScale;
            }
            co_flipping = null;
        }
            

    }
}
