using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Meal", menuName = "Tavern/Cooking/Meal")]
public class Meal : ScriptableObject
{
    public string mealName;
    public List<Ingredient> ingredients;
    public MiniGameHandler[] miniGames = new MiniGameHandler[3];
}
