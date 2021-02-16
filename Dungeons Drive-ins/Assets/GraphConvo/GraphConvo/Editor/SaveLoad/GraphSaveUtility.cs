using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

namespace GraphConvo
{
    public class GraphSaveUtility
    {
        private ConvoGraphView convoGraphView;
        private ConversationContainer conversationContainer;

        private List<Edge> edges => convoGraphView.edges.ToList();
        private List<BaseNode> nodes => convoGraphView.nodes.ToList().Cast<BaseNode>().ToList();

        public static GraphSaveUtility GetInstance(ConvoGraphView convoGraphView)
        {
            return new GraphSaveUtility
            {
                convoGraphView = convoGraphView
            };
        }

        public void SaveGraph(ConversationContainer conversationContainer)
        {
            if (!edges.Any())
            {
                Debug.Log("Graph doesn't contain edges");
                return;
            }

            this.conversationContainer = conversationContainer;
            ClearContainer();

            //Get an array of all the edges that is connected to an input port.
            Edge[] connectedPorts = edges.Where(x => x.input.node != null).ToArray();

            for (int i = 0; i < connectedPorts.Length; i++)
            {
                BaseNode outputNode = (BaseNode) connectedPorts[i].output.node;
                BaseNode inputNode = connectedPorts[i].input.node as BaseNode;

                conversationContainer.nodeLinks.Add(new NodeLinkData
                {
                    //The node that the edge is connected from.
                    baseNodeGuid = outputNode.guID,
                    portName = connectedPorts[i].output.portName,
                    //The node that the edge is connected to.
                    targetNodeGuid = inputNode.guID
                });
            }

            foreach (BaseNode node in nodes.Where(node=> node.nodeType != NodeType.Entry))
            {
                ConvoNodeData convoNodeData = new ConvoNodeData
                {
                    guid = node.guID,
                    nodeType = node.nodeType,
                    position = node.GetPosition().position
                };
                if (node.nodeType == NodeType.Dialogue)
                {
                    convoNodeData.dialogues = (node as DialogueNode).dialogues;
                }
                conversationContainer.convoNodeData.Add(convoNodeData);

            }
        }

        private void ClearContainer()
        {
            conversationContainer.convoNodeData = new List<ConvoNodeData>();
            conversationContainer.nodeLinks = new List<NodeLinkData>();
        }

        public void LoadGraph(ConversationContainer conversationContainer)
        {
            this.conversationContainer = conversationContainer;
            ClearGraph();
            CreateNodes();
            ConnectNodes();

            //Make sure that all dialogueNodes get their output port after loading.
            List<DialogueNode> dialogueNodes = nodes.Where(x => x.nodeType == NodeType.Dialogue).ToList().Cast<DialogueNode>().ToList();
            foreach (var dialogueNode in dialogueNodes)
            {
                if (dialogueNode.outputContainer.childCount < 1)
                    dialogueNode.AddOutputPort();
            }
        }

        private void ClearGraph()
        {
            nodes.Find(x => x.nodeType == NodeType.Entry).guID = conversationContainer.nodeLinks[0].baseNodeGuid;

            foreach (BaseNode node in nodes)
            {
                if (node.nodeType == NodeType.Entry) continue;

                edges.Where(x => x.input.node == node).ToList()
                    .ForEach(edge => convoGraphView.RemoveElement(edge));

                convoGraphView.RemoveElement(node);
            }
        }
        private void CreateNodes()
        {
            foreach (ConvoNodeData nodeData in conversationContainer.convoNodeData)
            {
                BaseNode node = convoGraphView.GenerateNode(nodeData.nodeType, nodeData.position, true);
                node.guID = nodeData.guid;

                var nodePorts = conversationContainer.nodeLinks.Where(x => x.baseNodeGuid == nodeData.guid).ToList();
                nodePorts.ForEach(x => node.AddOutputPort(x.portName));

                if (nodeData.nodeType == NodeType.Dialogue)
                    (node as DialogueNode).LoadDialogues(nodeData.dialogues);
            }
        }
        private void ConnectNodes()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                List<NodeLinkData> connections = conversationContainer.nodeLinks.Where(x => x.baseNodeGuid == nodes[i].guID).ToList();
                for (int j = 0; j < connections.Count; j++)
                {
                    string targetNodeGuid = connections[j].targetNodeGuid;
                    BaseNode targetNode = nodes.First(x => x.guID == targetNodeGuid);
                    LinkNodes(nodes[i].outputContainer[j].Q<Port>(),(Port) targetNode.inputContainer[0]);
                }
            }
        }
        private void LinkNodes(Port outputPort, Port inputPort)
        {
            Edge newEdge = new Edge
            {
                output = outputPort,
                input = inputPort
            };

            newEdge.input.Connect(newEdge);
            newEdge.output.Connect(newEdge);
            convoGraphView.Add(newEdge);
        }
    }
}
