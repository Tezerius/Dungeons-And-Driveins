using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameNumber : MiniGame
{
    [SerializeField] Button[] nbrButtons = new Button[0];

    private int currentButton = 0;


    public override void StartMiniGame()
    {
        base.StartMiniGame();
    
        for (int i = 0; i < nbrButtons.Length; i++)
        {
            int index = i;
            nbrButtons[i].onClick.AddListener(() => OnButtonClickCallback(index));
        }
    }

    private void OnButtonClickCallback(int index)
    {
        if (!miniGameRunning)
            return;

        if(index != currentButton)
        {
            ResetGame();
        }
        else
        {
            currentButton++;
            nbrButtons[index].image.color = Color.green;
        }

        if(currentButton >= nbrButtons.Length)
        {
            MiniGameComplete(true);
        }
    }

    private void ResetGame()
    {
        currentButton = 0;
        foreach (Button button in nbrButtons)
        {
            button.image.color = Color.white;
        }
    }
}
