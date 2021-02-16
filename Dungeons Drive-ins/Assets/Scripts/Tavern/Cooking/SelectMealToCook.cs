using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMealToCook : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] Button mealButtonPrefab;
    [Header("Components")]
    [SerializeField] RectTransform mealSelectionContainer;
    [SerializeField] RectTransform mealButtonContainer;
    [Header("External References")]
    [SerializeField] AvaliableMealsDataObj avaliableMeals;

    private List<Button> mealChoiceButtons = new List<Button>();

    public Action<Meal> selectedMealAction;

    private void Awake()
    {
        mealSelectionContainer.gameObject.SetActive(false);
    }

    public void SelectMeal()
    {
        mealSelectionContainer.gameObject.SetActive(true);

        for (int i = 0; i < avaliableMeals.knownMealRecepies.Count; i++)
        {
            Meal meal = avaliableMeals.knownMealRecepies[i];
            Button mealChoiceButton = Instantiate(mealButtonPrefab, mealButtonContainer);
            mealChoiceButtons.Add(mealChoiceButton);
            mealChoiceButton.GetComponentInChildren<Text>().text = meal.mealName;
            mealChoiceButton.onClick.AddListener(() => MealSelected(meal));
            mealChoiceButton.onClick.AddListener(() => RemoveButtons());
        }
    }

    private void MealSelected(Meal selectedMeal)
    {
        selectedMealAction?.Invoke(selectedMeal);
        mealSelectionContainer.gameObject.SetActive(false);
    }

    private void RemoveButtons()
    {
        foreach (Button button in mealChoiceButtons)
        {
            Destroy(button.gameObject);
        }
        mealChoiceButtons = new List<Button>();
    }

}
