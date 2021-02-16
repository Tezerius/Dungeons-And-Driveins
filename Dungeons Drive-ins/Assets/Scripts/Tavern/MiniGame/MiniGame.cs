using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public abstract class MiniGame : MonoBehaviour
{
    protected bool miniGameRunning;

    public Action<bool> ActionMiniGameCompleted;

    public virtual void StartMiniGame()
    {
        miniGameRunning = true;
    }

    public void MiniGameComplete(bool success)
    {
        miniGameRunning = false;
        ActionMiniGameCompleted?.Invoke(success);
    }

}
