using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GraphConvo;
using System.Linq;
using System;

public class ConversationManager : MonoBehaviour
{
    [Header("ConversationSettings")]
    [SerializeField] float delayBeforeNewConvo = 2f;
    [SerializeField] float characterTypingSpeed = 0.1f;

    [Header("Components")]
    [SerializeField] DialogueBoxManager dialogueBoxManager;
    [SerializeField] Text speaker;
    [SerializeField] Text conversationBox;
    [Header("Choice")]
    [SerializeField] Button choicePrefab;
    [SerializeField] Transform buttonContainer;

    private CharacterImageManager characterImageManager;

    //Public for testing
    private ConversationContainer currentConversation;

    private List<Button> choiceButtons = new List<Button>();

    private ConvoNodeData PreviousNodeData { get; set; }

    private void Awake()
    {
        characterImageManager = FindObjectOfType<CharacterImageManager>();
    }

    public void StartConversation(ConversationContainer newConversation)
    {
        currentConversation = newConversation;
        //Procede to the first node in the graph.
        ContinueToNextNode(newConversation.nodeLinks.First().targetNodeGuid);
    }

    private void ContinueToNextNode(string guid)
    {
        if (guid == "")
        {
            StartCoroutine(Utility.WaitAndExecuteFunction(EndConversation, 2f));
            return;
        }
        ConvoNodeData convoNodeData = currentConversation.convoNodeData.Find(x => x.guid == guid);
        switch (convoNodeData.nodeType)
        {
            case NodeType.Entry:
                break;
            case NodeType.Dialogue:
                HandleDialogueNode(convoNodeData);
                break;
            case NodeType.Choice:
                HandleChoiceNode(convoNodeData);
                break;
            default:
                break;
        }
        PreviousNodeData = convoNodeData;
    }

    private void EndConversation()
    {
        characterImageManager.SlideNPCImage(null, false);
        StartCoroutine(dialogueBoxManager.SlideChatBox(false));
    }

    #region Handle Dialogue Nodes
    private void HandleDialogueNode(ConvoNodeData convoNodeData)
    {
        StartCoroutine(ShowDialogues(convoNodeData));
    }

    IEnumerator ShowDialogues(ConvoNodeData convoNodeData)
    {
        if (PreviousNodeData != null)
            if (PreviousNodeData.nodeType == NodeType.Dialogue)
                yield return new WaitForSeconds(delayBeforeNewConvo);

        if (!dialogueBoxManager.ConversationBox.gameObject.activeSelf)
        {
            StartCoroutine(dialogueBoxManager.SlideChatBox(true));
        }

        for (int i = 0; i < convoNodeData.dialogues.Count; i++)
        {
            Dialogue currentDialogue = convoNodeData.dialogues[i];
            speaker.text = currentDialogue.conversationCharacter.characterName;
            characterImageManager.SlideNPCImage(currentDialogue.conversationCharacter.characterImg);

            yield return new WaitUntil(() => dialogueBoxManager.slidingChatbox == false);

            StartCoroutine(ShowDialogueText(currentDialogue.dialogueText));
            yield return new WaitUntil(() => showDialogueTextRoutineRunning == false);
            if (i != convoNodeData.dialogues.Count - 1)
                yield return new WaitForSeconds(delayBeforeNewConvo);
        }

        NodeLinkData nodeLinkData = currentConversation.nodeLinks.Find(x => x.baseNodeGuid == convoNodeData.guid);
        if (nodeLinkData != null)
            ContinueToNextNode(nodeLinkData.targetNodeGuid);
        else
            ContinueToNextNode("");
    }

    private bool showDialogueTextRoutineRunning;
    IEnumerator ShowDialogueText(string dialogueText)
    {
        showDialogueTextRoutineRunning = true;
        conversationBox.text = "";
        foreach (char letter in dialogueText)
        {
            conversationBox.text += letter;
            yield return new WaitForSeconds(characterTypingSpeed);
        }
        showDialogueTextRoutineRunning = false;
    }
    #endregion

    #region Handle Choice Buttons
    private void HandleChoiceNode(ConvoNodeData convoNodeData)
    {
        var choices = currentConversation.nodeLinks.Where(x => x.baseNodeGuid == convoNodeData.guid);
        foreach (NodeLinkData choice in choices)
        {
            Button choiceButton = Instantiate(choicePrefab, buttonContainer);
            choiceButtons.Add(choiceButton);
            choiceButton.GetComponentInChildren<Text>().text = choice.portName;
            choiceButton.onClick.AddListener(() => ContinueToNextNode(choice.targetNodeGuid));
            choiceButton.onClick.AddListener(() => RemoveButtons());
        }
    }

    private void RemoveButtons()
    {
        foreach (Button button in choiceButtons)
        {
            Destroy(button.gameObject);
        }
        choiceButtons = new List<Button>();
    }
    #endregion
}
