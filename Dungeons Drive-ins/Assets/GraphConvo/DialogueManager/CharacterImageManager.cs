using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterImageManager : MonoBehaviour
{
    [SerializeField] float lerpTime = 0.5f;
    [Header("NPC")]
    [SerializeField] Image nPCImage;
    [SerializeField] RectTransform npcStart;
    [SerializeField] RectTransform npcDestination;
    [Header("Main Character")]
    [SerializeField] Image mainCharacterImage;

    public void HideAllCharacters()
    {
        nPCImage.enabled = false;
        mainCharacterImage.enabled = false;
    }

    public void SlideNPCImage(Sprite npcSprite = null, bool slideIn = true)
    {
        if(slideIn)
        {
            if (nPCImage.sprite == npcSprite)
            {
                return;
            }
            nPCImage.enabled = true;
            nPCImage.sprite = npcSprite;
            nPCImage.rectTransform.localPosition = npcStart.localPosition;

            slideImageRoutine = SlideImage(nPCImage, npcStart, npcDestination, lerpTime);
            StartCoroutine(slideImageRoutine);
        }
        else
        {
            StopCoroutine(slideImageRoutine);
            slideImageRoutine = SlideImage(nPCImage, npcDestination, npcStart, lerpTime);
            StartCoroutine(slideImageRoutine);
        }
    }

    IEnumerator slideImageRoutine;
    IEnumerator SlideImage(Image image, RectTransform start, RectTransform destination, float lerpDuration)
    {
        float timer = 0f;
        while (image.rectTransform.localPosition != destination.localPosition)
        {
            image.rectTransform.localPosition = Vector3.Lerp(start.localPosition, destination.localPosition, timer / lerpDuration);
            timer += Time.deltaTime;

            yield return null;
        }
    }
}
