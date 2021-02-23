using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] RectTransform cookingContainer;
    [SerializeField] Text mealNameText;
    [SerializeField] RectTransform miniGameContainer;

    private SelectMealToCook selectMealToCook;

    private Meal mealToCook;
    private List<MiniGameHandler> miniGames = new List<MiniGameHandler>();

    void Start()
    {
        selectMealToCook = FindObjectOfType<SelectMealToCook>();
        selectMealToCook.selectedMealAction += StartNewMiniGameSet;
    }

    private void StartNewMiniGameSet(Meal selectedMeal)
    {
        cookingContainer.gameObject.SetActive(true);

        mealToCook = selectedMeal;
        mealNameText.text = selectedMeal.mealName;

        InstantientMiniGames(selectedMeal);
    }

    private void InstantientMiniGames(Meal mealToCook)
    {
        foreach (MiniGameHandler miniGame in mealToCook.miniGames)
        {
            if (miniGame != null)
            {
                MiniGameHandler miniGameHandler = Instantiate(miniGame, miniGameContainer);
                miniGameHandler.StartMiniGame();
            }
        }
    }
}
