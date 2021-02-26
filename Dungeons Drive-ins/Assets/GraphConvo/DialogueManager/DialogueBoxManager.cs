using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GraphConvo;
using System;
using UnityEngine.Events;

public class DialogueBoxManager : MonoBehaviour
{
    [Header("Sliding Components")]
    [SerializeField] RectTransform conversationBox;
    [SerializeField] RectTransform startPos;
    [SerializeField] RectTransform destinationPos;
    [SerializeField] float slideDuration = 0.5f;
    [Header("Dialogue Components")]
    [SerializeField] Text speakerText;
    [SerializeField] Text dialogueTextElement;
    [Header("Text Settings")]
    [SerializeField] float delayBeforeNewConvo = 2f;
    [SerializeField] float characterTypingSpeed = 0.1f;


    public RectTransform ConversationBox { get => conversationBox; }

    public Action<Dialogue> OnNewDialogueStarted;
    public Action<ConvoNodeData> OnDialoguesFinished;
    private ConvoNodeData convoNodeData;

    private Dialogue[] dialogues;
    private int nextDialogueIndex;

    private void Awake()
    {
        conversationBox.localPosition = startPos.localPosition;
        conversationBox.gameObject.SetActive(false);
    }

    public void ShowDialogues(ConvoNodeData convoNodeData, bool clickToContinue, Action<Dialogue> OnNewDialogueStarted, Action<ConvoNodeData> OnDialoguesFinished)
    {
        gameObject.SetActive(true);
        nextDialogueIndex = 0;
        this.convoNodeData = convoNodeData;
        dialogues = convoNodeData.dialogues.ToArray();

        this.OnNewDialogueStarted = OnNewDialogueStarted;
        this.OnDialoguesFinished = OnDialoguesFinished;

        if (conversationBox.localPosition != destinationPos.localPosition)
        {
            StartCoroutine(SlideChatBox(true));
        }
        if (clickToContinue)
        {
            StartNewDialogue(0);
        }
        else
            StartCoroutine(ShowDialoguesAutomatically());
    }

    public void HideDialogueBox()
    {
        StartCoroutine(SlideChatBox(false));
    }

    public void Continue()
    {
        if (nextDialogueIndex >= dialogues.Length)
            return;

        if (showDialogueTextRoutineRunning)
        {
            StopCoroutine(showDialogueTextRoutine);
        }

        if(dialogueTextElement.text != dialogues[nextDialogueIndex].dialogueText)
        {
            ShowDialogueTextInstant(dialogues[nextDialogueIndex].dialogueText);
            Debug.Log("Instant");
        }
        else if(dialogueTextElement.text == dialogues[nextDialogueIndex].dialogueText)
        {
            nextDialogueIndex++;
            if (nextDialogueIndex >= dialogues.Length)
            {
                CheckIfDialogueFinished();
                return;
            }
            else
            {
                Debug.Log("New");
                StartNewDialogue(nextDialogueIndex);
            }
        }
    }

    private IEnumerator ShowDialoguesAutomatically()
    {
        for (int i = 0; i < dialogues.Length; i++)
        {
            nextDialogueIndex = i;
            StartNewDialogue(nextDialogueIndex);
            
            yield return new WaitUntil(() => showDialogueTextRoutineRunning == false);

            if (i != dialogues.Length - 1)
                yield return new WaitForSeconds(delayBeforeNewConvo);

            if (i >= dialogues.Length - 1)
            {
                Debug.Log("End of dialogue");
            }
        }
    }

    private void StartNewDialogue(int index)
    {
        Dialogue currentDialogue = dialogues[index];
        OnNewDialogueStarted?.Invoke(currentDialogue);

        speakerText.text = currentDialogue.conversationCharacter.characterName;

        showDialogueTextRoutine = ShowDialogueTextRoutine(dialogues[nextDialogueIndex].dialogueText);

        StartCoroutine(showDialogueTextRoutine);
    }

    private IEnumerator showDialogueTextRoutine;
    private bool showDialogueTextRoutineRunning;
    IEnumerator ShowDialogueTextRoutine(string dialogueText)
    {
        showDialogueTextRoutineRunning = true;
        dialogueTextElement.text = "";
        foreach (char letter in dialogueText)
        {
            dialogueTextElement.text += letter;
            yield return new WaitForSeconds(characterTypingSpeed);
        }
        showDialogueTextRoutineRunning = false;
    }

    private void ShowDialogueTextInstant(string dialogueText)
    {
        dialogueTextElement.text = dialogueText;
    }

    private void CheckIfDialogueFinished()
    {
        if (nextDialogueIndex >= dialogues.Length)
        {
            OnDialoguesFinished?.Invoke(convoNodeData);
            Debug.Log("Finished");
            return;
        }
    }

    private IEnumerator SlideChatBox(bool slideIn)
    {
        conversationBox.gameObject.SetActive(true);

        float timer = 0f;
        Vector3 start;
        Vector3 destination;

        if(slideIn)
        {
            start = startPos.localPosition;
            destination = destinationPos.localPosition;
        }
        else
        {
            start = destinationPos.localPosition;
            destination = startPos.localPosition;
        }

        while (conversationBox.localPosition != destination)
        {
            conversationBox.localPosition = Vector3.Lerp(start, destination, (timer / slideDuration));
            timer += Time.deltaTime;

            yield return null;
        }
        if(!slideIn) 
            conversationBox.gameObject.SetActive(false);
    }
}
