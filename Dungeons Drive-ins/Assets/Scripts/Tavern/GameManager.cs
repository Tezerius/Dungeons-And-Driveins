using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GraphConvo.ConversationContainer starterConversation;

    [SerializeField] ConversationManager conversationManager;
    [SerializeField] SelectMealToCook selectMealToCook;

    void Start()
    {
        conversationManager.StartConversation(starterConversation);
        StartCoroutine(WaitAndExecuteFunction(selectMealToCook.SelectMeal, 3f));
    }

    private IEnumerator WaitAndExecuteFunction(Action functionToExecute, float delayBeforeExecution)
    {
        yield return new WaitForSeconds(delayBeforeExecution);
        functionToExecute.Invoke();
    }

}
