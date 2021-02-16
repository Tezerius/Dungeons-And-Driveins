using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;
using System.Linq;

namespace GraphConvo
{
    public class ConvoGraphView : GraphView
    {
        private GraphConvoEditor graphConvoEditor;
        private NodeSearchWindow searchWindow;

        public ConvoGraphView(GraphConvoEditor graphConvoEditor)
        {
            this.graphConvoEditor = graphConvoEditor;

            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);

            StyleSheet styleSheet = Resources.Load<StyleSheet>("GraphConvoView");
            styleSheets.Add(styleSheet);

            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            GenerateNode(NodeType.Entry, Vector2.one * 50);

            AddSearchWindow();
        }

        public void AddSearchWindow()
        {
            searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
            searchWindow.Configure(graphConvoEditor, this);
            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach((port) => 
            { 
                if (startPort != port && startPort.node != port.node)
                {
                    compatiblePorts.Add(port);
                }
            }
            );

            return compatiblePorts;
        }

        public BaseNode GenerateNode(NodeType type, Vector2 position, bool loadingNodes = false)
        {
            BaseNode node = null;
            if(type == NodeType.Entry)
            {
                node = new EntryNode
                {
                    guID = Guid.NewGuid().ToString(),
                    convoGraphView = this,
                };

            }
            else if(type == NodeType.Dialogue)
            {
                node = new DialogueNode
                {
                    guID = Guid.NewGuid().ToString(),
                    convoGraphView = this,
                };
            }
            else if (type == NodeType.Choice)
            {
                node = new ChoiceNode
                {
                    guID = Guid.NewGuid().ToString(),
                    convoGraphView = this,
                };
            }

            if (node != null)
            {
                if (!loadingNodes)
                    node.AddOutputPort();
                AddElement(node);
                node.nodeType = type;
                node.SetPosition(new Rect(position, default));
            }
            return node;
        }

        public void RemoveEdge(BaseNode node, Port port)
        {

            var targetEdge = edges.ToList()
                .Where(x => x.output.portName == port.portName && x.output.node == port.node);
            if (targetEdge.Any())
            {
                var edge = targetEdge.First();
                edge.input.Disconnect(edge);
                RemoveElement(targetEdge.First());
            }
        }
    }

}
