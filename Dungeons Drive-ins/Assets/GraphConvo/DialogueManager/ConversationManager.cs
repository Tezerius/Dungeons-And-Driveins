using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GraphConvo;
using System.Linq;
using System;

public class ConversationManager : MonoBehaviour
{
    [Header("Conversation Settings")]
    [SerializeField] bool clickToContinue = true;
    [SerializeField] float delayBeforeNewConvo = 2f;

    [Header("Components")]
    [SerializeField] DialogueBoxManager dialogueBoxManager;
    [Header("Choice")]
    [SerializeField] Button choicePrefab;
    [SerializeField] Transform buttonContainer;

    [Header("Continue Button")]
    [SerializeField] Button continueButton;

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
        continueButton.onClick.AddListener(() => ContinueButtonCallback());
        currentConversation = newConversation;
        //Procede to the first node in the graph.
        ContinueToNextNode(newConversation.nodeLinks.First().targetNodeGuid);
    }

    private void ContinueToNextNode(string guid)
    {
        if (guid == "")
        {
            if(clickToContinue)
            {
                EndConversation();
                return;
            }
            else
            {
                StartCoroutine(Utility.WaitAndExecuteFunction(EndConversation, 2f));
                return;
            }
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
        dialogueBoxManager.HideDialogueBox();
        continueButton.onClick.RemoveAllListeners();
    }

    private void ContinueButtonCallback()
    {
        dialogueBoxManager.Continue();
    }

    #region Handle Dialogue Nodes
    private void HandleDialogueNode(ConvoNodeData convoNodeData)
    {
        dialogueBoxManager.ShowDialogues(convoNodeData, clickToContinue, (Dialogue dialogue) => OnNewDialogueStartedCallback(dialogue), (ConvoNodeData x) => OnDialoguesFinishedCallback(x));
    }

    private void OnNewDialogueStartedCallback(Dialogue dialogue)
    {
        characterImageManager.SlideNPCImage(dialogue.conversationCharacter.characterImg);
    }

    private void OnDialoguesFinishedCallback(ConvoNodeData convoNodeData)
    {
        NodeLinkData nodeLinkData = currentConversation.nodeLinks.Find(x => x.baseNodeGuid == convoNodeData.guid);
        if (nodeLinkData != null)
            ContinueToNextNode(nodeLinkData.targetNodeGuid);
        else
            ContinueToNextNode("");
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
