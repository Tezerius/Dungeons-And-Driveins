using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameChop : MiniGame
{
    [Header("Components")]
    [SerializeField] RectTransform startSpawnPos;
    [SerializeField] RectTransform endPosition;
    [Header("Food")]
    [SerializeField] List<FoodForChop> foodsToChop;
    [SerializeField] float slideDuration = 10f;
    [SerializeField] int nbrOfSlice = 5;
    [SerializeField] float spawnInterval = 4;
    [SerializeField] int nbrMissesForLoss = 2;


    private List<FoodForChop> currentFoods = new List<FoodForChop>();
    private int nbrOfSliceCounter;
    private int nbrMisses;

    private Button[] chopButtons;

    public override void StartMiniGame()
    {
        base.StartMiniGame();

        chopButtons = GetComponentsInChildren<Button>();

        foreach (Button button in chopButtons)
        {
            button.onClick.AddListener(() => OnChopButtonClick(button));
        }

        foodSliceRoutine = FoodSlice();
        StartCoroutine(foodSliceRoutine);
    }

    private IEnumerator foodSliceRoutine;
    private IEnumerator FoodSlice()
    {
        while (miniGameRunning)
        {
            SpawnFoodToChop();

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnFoodToChop()
    {
        FoodForChop spawnedFood = Instantiate(foodsToChop[Random.Range(0, foodsToChop.Count)], startSpawnPos.parent.transform);
        spawnedFood.Init(startSpawnPos, endPosition, slideDuration, RemoveFoodFromList);
        currentFoods.Add(spawnedFood);
    }

    private void OnChopButtonClick(Button chopButton)
    {
        List<FoodForChop> bufferFoodList = new List<FoodForChop>();

        RectTransform rectButton = chopButton.GetComponent<RectTransform>();

        foreach (FoodForChop food in currentFoods)
        {
            if(CheckRectOverlap(rectButton, food.GetComponent<RectTransform>()))
            { 
                food.Slice();
                nbrOfSliceCounter++;
                bufferFoodList.Add(food);

                if(nbrOfSliceCounter >= nbrOfSlice)
                {
                    WonMiniGame();
                }
            }
        }
        foreach (FoodForChop food in bufferFoodList)
        {
            RemoveFoodFromList(food);
        }
    }

    private void WonMiniGame()
    {
        foreach (FoodForChop foodForChop in currentFoods)
        {
            Destroy(foodForChop.gameObject);
        }
        MiniGameComplete(true);
    }

    private bool CheckRectOverlap(RectTransform rectTransform1, RectTransform rectTransform2)
    {
        Bounds bounds = new Bounds(rectTransform1.localPosition, rectTransform1.rect.size);
        Bounds bounds1 = new Bounds(rectTransform2.localPosition, rectTransform2.rect.size);

        return bounds.Intersects(bounds1);
    }

    public void RemoveFoodFromList(FoodForChop foodForChop)
    {
        if (!currentFoods.Contains(foodForChop))
            return;
        else if (!foodForChop.IsSliced)
            MissedFood();
        currentFoods.Remove(foodForChop);

    }

    private void MissedFood()
    {
        nbrMisses++;
        if (nbrMisses >= nbrMissesForLoss)
        {
            MiniGameComplete(false);
        }
    }
}
