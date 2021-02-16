using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace GraphConvo
{
    public class ChoiceNode : BaseNode
    {
        public ChoiceNode()
        {
            title = "Choice";
            editableOutputPorts = true;

            SetPosition(new Rect(300, 200, 100, 150));

            AddInputPort();

            //AddOutputPort();
            GenerateDialogueFieldContainer();
        }

        private void GenerateDialogueFieldContainer()
        {
            Button addChoiceButton = new Button(() => AddOutputPort());
            addChoiceButton.text = "Add Choice";

            titleContainer.Add(addChoiceButton);
        }

    }
}