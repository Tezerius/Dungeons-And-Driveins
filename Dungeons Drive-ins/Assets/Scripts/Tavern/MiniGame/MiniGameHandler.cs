using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameHandler : MonoBehaviour
{
    [Header("Handler")]
    [SerializeField] string miniGameName;
    [SerializeField] Text miniGameNameText;
    [SerializeField] Image successOrFailImage;
    [SerializeField] Sprite successSprite;
    [SerializeField] Sprite failSprite;
    [Header("Mini-game")]
    [SerializeField] MiniGame miniGame;

    public Action<bool> ActionMiniGameCompleted;

    public void StartMiniGame()
    {
        if (miniGame == null)
            miniGame = GetComponentInChildren<MiniGame>();

        miniGameNameText.text = miniGameName;

        miniGame.ActionMiniGameCompleted += MiniGameCompletedCallback;
        miniGame.StartMiniGame();
    }

    private void MiniGameCompletedCallback(bool success)
    {
        SetSucessImage(success);
        ActionMiniGameCompleted?.Invoke(success);
    }

    private void SetSucessImage(bool success)
    {
        successOrFailImage.enabled = true;
        if (success)
        {
            successOrFailImage.sprite = successSprite;
        }
        else
        {
            successOrFailImage.sprite = failSprite;
        }
    }

}
