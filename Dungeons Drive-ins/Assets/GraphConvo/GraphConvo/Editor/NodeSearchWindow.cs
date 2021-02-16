using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphConvo
{
    public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private GraphConvoEditor graphConvoEditor;
        private ConvoGraphView graphConvoView;

        private Texture2D icon;

        public void Configure(GraphConvoEditor graphConvoEditor, ConvoGraphView graphConvoView)
        {
            this.graphConvoEditor = graphConvoEditor;
            this.graphConvoView = graphConvoView;

            icon = new Texture2D(1, 1);
            icon.SetPixel(0, 0, new Color(0, 0, 0, 0));
            icon.Apply();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Graph Conversation"), 0),
                new SearchTreeGroupEntry(new GUIContent("Nodes"), 1),
                
                AddNodeSearch("Dialogue Node", new DialogueNode()),
                AddNodeSearch("Choice Node", new ChoiceNode())
            };


            return tree;
        }

        private SearchTreeEntry AddNodeSearch(string nodeName, BaseNode node)
        {
            SearchTreeEntry tmp = new SearchTreeEntry(new GUIContent(nodeName,icon))
            {
                level = 2,
                userData = node
            };

            return tmp;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            Vector2 mousePosition = graphConvoEditor.rootVisualElement.ChangeCoordinatesTo(graphConvoEditor.rootVisualElement.parent, context.screenMousePosition - graphConvoEditor.position.position);

            Vector2 graphMousePosition = graphConvoView.contentViewContainer.WorldToLocal(mousePosition);

            return CheckForNodeType(SearchTreeEntry, graphMousePosition);
        }

        private bool CheckForNodeType(SearchTreeEntry searchTreeEntry, Vector2 position)
        {
            switch (searchTreeEntry.userData)
            {
                case DialogueNode node:
                    graphConvoView.GenerateNode(NodeType.Dialogue, position);
                    return true;
                case ChoiceNode node:
                    graphConvoView.GenerateNode(NodeType.Choice, position);
                    return true;
                default:
                    break;
            }
            return false;
        }
    }
}