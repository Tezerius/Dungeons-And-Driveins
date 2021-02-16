using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphConvo
{
    [CreateAssetMenu(fileName = "ConversationContainer", menuName = "Conversation/Container")]
    [System.Serializable]
    public class ConversationContainer : ScriptableObject
    {
        public List<ConversationCharacter> characters;

        public List<NodeLinkData> nodeLinks = new List<NodeLinkData>();
        public List<ConvoNodeData> convoNodeData = new List<ConvoNodeData>();
    }
}

