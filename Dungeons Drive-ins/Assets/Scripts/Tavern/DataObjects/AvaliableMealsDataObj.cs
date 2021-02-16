using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Avliable Meals Data Obj", menuName = "Tavern/Data Objects/Avaliable Meals Data Obj")]
public class AvaliableMealsDataObj : ScriptableObject
{
    public List<Meal> knownMealRecepies;
}
