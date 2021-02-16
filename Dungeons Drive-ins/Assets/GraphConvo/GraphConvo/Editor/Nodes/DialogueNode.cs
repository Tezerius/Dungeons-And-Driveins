using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

namespace GraphConvo
{
    public class DialogueNode : BaseNode
    {
        public List<Dialogue> dialogues = new List<Dialogue>();

        VisualElement dialogueFieldContainer;

        public DialogueNode()
        {
            title = "Conversation";

            SetPosition(new Rect(300, 200, 100, 150));

            AddInputPort();
            //AddOutputPort();
            GenerateDialogueFieldContainer();
        }

        private void GenerateDialogueFieldContainer()
        {
            dialogueFieldContainer = new VisualElement();
            dialogueFieldContainer.style.flexDirection = FlexDirection.Column;

            Button addFieldButton = new Button( () => AddDialogueField() );
            addFieldButton.text = "Add Dialogue";

            titleContainer.Add(addFieldButton);

            mainContainer.Add(dialogueFieldContainer);
        }

        private void AddDialogueField(Dialogue dialogueToLoad = null)
        {
            VisualTreeAsset visualTreeAsset = Resources.Load<VisualTreeAsset>("DialogueContainer");
            VisualElement visualElement = visualTreeAsset.CloneTree();

            Dialogue dialogue;
            if (dialogueToLoad != null)
            {
                dialogue = dialogueToLoad;
                dialogue.conversationCharacter = dialogueToLoad.conversationCharacter;
            }
            else
                dialogue = new Dialogue();

            Button deleteDialogueField = (Button)visualElement.Query<VisualElement>("Delete-Dialogue");

            deleteDialogueField.clicked += () => RemoveDialogueField(visualElement, dialogue);


            ObjectField characterObjectField = visualElement.Q<ObjectField>("CharacterObjectField");
            characterObjectField.objectType = typeof(ConversationCharacter);
            characterObjectField.RegisterValueChangedCallback((x) => CharacterObjectFieldCallback(dialogue, (ConversationCharacter)x.newValue));
            if (dialogue.conversationCharacter != null) characterObjectField.SetValueWithoutNotify(dialogue.conversationCharacter);

            TextField conversationText = visualElement.Query<TextField>("Conversation-Text");
            conversationText.SetValueWithoutNotify(dialogue.dialogueText);
            conversationText.RegisterValueChangedCallback(evt => dialogue.dialogueText = evt.newValue);

            dialogues.Add(dialogue);
            dialogueFieldContainer.Add(visualElement);
        }

        private void RemoveDialogueField(VisualElement elementToRemove, Dialogue dialogue)
        {
            dialogues.Remove(dialogue);
            dialogueFieldContainer.Remove(elementToRemove);
        }

        private void CharacterObjectFieldCallback(Dialogue dialogue, ConversationCharacter conversationCharacter)
        {
            dialogue.conversationCharacter = conversationCharacter;
        }

        public void LoadDialogues(List<Dialogue> dialogues)
        {
            foreach (Dialogue dialogue in dialogues)
            {
                AddDialogueField(dialogue);
            }
        }

    }

}
