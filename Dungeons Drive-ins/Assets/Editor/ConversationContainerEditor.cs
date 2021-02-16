using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

//[CustomEditor(typeof(ConversationContainer))]
public class ConversationContainerEditor : Editor
{

    //public override VisualElement CreateInspectorGUI()
    //{
    //    return base.CreateInspectorGUI();
    //}

    //public override void OnInspectorGUI()
    //{
    //    base.OnInspectorGUI();

    //    ConversationContainer conversationContainer = (ConversationContainer)target;

    //    if (GUI.changed)
    //    {
    //        if (conversationContainer.conversationElement.Count > 0)
    //        {
    //            for (int i = 0; i < conversationContainer.conversationElement.Count; i++)
    //            {
    //                DialogueContainer dialogueContainer;
    //                if (conversationContainer.conversationElement[i] is DialogueContainer)
    //                    dialogueContainer = (DialogueContainer)conversationContainer.conversationElement[i];
    //                else
    //                    return;
    //                //if (dialogueContainer.dialogues.Count < 1) return;
    //                for (int j = 0; j < dialogueContainer.dialogues.Count; j++)
    //                {
    //                    Dialogue dialogue = dialogueContainer.dialogues[j];
    //                    dialogue.name = dialogue.character ? dialogue.character.characterName : "";
    //                }
    //            }
    //        }
    //    }

    //    if (GUILayout.Button("Add Dialogue"))
    //    {
    //        conversationContainer.conversationElement.Add(new DialogueContainer());
    //    }
    //    if (GUILayout.Button("Add Choice"))
    //    {
    //        conversationContainer.conversationElement.Add(new DialogueChoice());
    //    }
    //}


}
