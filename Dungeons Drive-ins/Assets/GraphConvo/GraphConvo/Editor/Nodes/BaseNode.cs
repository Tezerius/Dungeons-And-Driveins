using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphConvo
{
    public class BaseNode : Node
    {
        public NodeType nodeType;
        public string guID;
        public ConvoGraphView convoGraphView;
        public bool editableOutputPorts = false;

        public void AddOutputPort(string portName = "")
        {
            Port outputPort = GenereatePort();
            if (portName == "")
                outputPort.portName = "Output";
            else
                outputPort.portName = portName;

            if (editableOutputPorts)
            {
                TextField choiceText = new TextField();
                choiceText.name = "ChoiceText";
                choiceText.RegisterValueChangedCallback(evt => outputPort.portName = evt.newValue);
                choiceText.SetValueWithoutNotify(portName);

                outputPort.contentContainer.Add(new Label("  "));

                outputPort.contentContainer.Add(choiceText);
                Button deletePortButton = new Button(() => RemovePort(this, outputPort))
                { 
                    text = "X" 
                };
                outputPort.contentContainer.Add(deletePortButton);
                VisualElement oldLable = outputPort.contentContainer.Q<Label>("type");
                outputPort.contentContainer.Remove(oldLable);
            }

            outputContainer.Add(outputPort);
            RefreshExpandedState();
            RefreshPorts();
        }

        protected void AddInputPort()
        {
            Port inputPort = GenereatePort(Direction.Input, Port.Capacity.Multi);
            inputPort.portName = "Input";
            inputContainer.Add(inputPort);
            RefreshExpandedState();
            RefreshPorts();
        }

        private Port GenereatePort(Direction portDirection = Direction.Output, Port.Capacity capacity = Port.Capacity.Single)
        {
            Port port = InstantiatePort(Orientation.Horizontal,portDirection, capacity, typeof(float));
            return port;
        }

        private void RemovePort(BaseNode node, Port port)
        {
            convoGraphView.RemoveEdge(node, port);

            node.outputContainer.Remove(port);
            RefreshExpandedState();
            RefreshPorts();
        }
    }
}
