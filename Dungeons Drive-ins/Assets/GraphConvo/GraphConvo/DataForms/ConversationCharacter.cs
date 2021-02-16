using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConversationCharacter", menuName = "Conversation/Character")]
public class ConversationCharacter : ScriptableObject
{
    public string characterName = "Name";
    public Sprite characterImg;
}
