using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphConvo
{
    public enum NodeType { Entry, Dialogue, Choice }

    [System.Serializable]
    public class ConvoNodeData 
    {
        public NodeType nodeType;
        public string guid;
        public Vector2 position;

        public List<Dialogue> dialogues;
    }
}
