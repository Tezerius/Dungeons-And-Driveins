using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameFlip : MiniGame
{
    [Header("Settings")]
    [SerializeField] float timeBetweenFlips = 5f;
    [SerializeField] int nbrFlipsNeeded = 5;

    [SerializeField] Color healthyColor;
    [SerializeField] Color burnColor;

    [Header("Components")]
    [SerializeField] Button flipButton;

    private float timer;
    private int nbrFlipsDone;

    public override void StartMiniGame()
    {
        base.StartMiniGame();

        flipButton.onClick.AddListener(() => OnFlipButtonClick());
        flipButton.image.color = healthyColor;

        meatCookingRoutine = MeatCooking();
        StartCoroutine(meatCookingRoutine);
    }

    public void OnFlipButtonClick()
    {
        if (!miniGameRunning)
            return;

        if(timer > timeBetweenFlips / 2)
        {
            timer = 0;
            nbrFlipsDone++;
        }
        else
        {
            timer = 0;
        }

        if(nbrFlipsDone >= nbrFlipsNeeded)
        {
            MiniGameComplete(true);
        }
    }

    private IEnumerator meatCookingRoutine;
    private IEnumerator MeatCooking()
    {
        while(miniGameRunning)
        {
            if (timer >= timeBetweenFlips)
            {
                MiniGameComplete(false);
            }

            flipButton.image.color = Color.Lerp(healthyColor, burnColor, timer / timeBetweenFlips);
            timer += Time.deltaTime;

            yield return null;
        }
    }
}
