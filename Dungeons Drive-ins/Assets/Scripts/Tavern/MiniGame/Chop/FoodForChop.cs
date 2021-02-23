using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodForChop : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite slicedSprite;

    public bool IsSliced { get; private set; }

    private RectTransform rectTransform;
    private RectTransform startPosition;
    private RectTransform endPosition;

    private float duration;

    private float timer;

    private Action<FoodForChop> OnFoodDestroy;

    public void Init(RectTransform startPosition, RectTransform endPosition, float duration, Action<FoodForChop> OnFoodDestroy)
    {
        this.endPosition = endPosition;
        rectTransform = GetComponent<RectTransform>();
        this.startPosition = startPosition;

        this.duration = duration;

        this.OnFoodDestroy = OnFoodDestroy;
    }

    private void Update()
    {
        if (endPosition == null)
            return;

        if (rectTransform.localPosition.x <= endPosition.localPosition.x)
        {
            OnFoodDestroy?.Invoke(this);
            Destroy(gameObject);
        }

        timer += Time.deltaTime;

        rectTransform.localPosition = Vector3.Lerp(startPosition.localPosition, endPosition.localPosition, timer / duration);
    }

    public void Slice()
    {
        image.sprite = slicedSprite;
        IsSliced = true;
    }

}
