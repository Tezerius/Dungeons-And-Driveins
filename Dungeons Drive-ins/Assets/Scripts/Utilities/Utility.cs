using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility 
{
    public static IEnumerator WaitAndExecuteFunction(Action functionToExecute, float delayBeforeExecution)
    {
        yield return new WaitForSeconds(delayBeforeExecution);
        functionToExecute.Invoke();
    }
}
