using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace GraphConvo
{
    public class EntryNode : BaseNode
    {
        public EntryNode()
        {
            title = "Entry";

            capabilities &= ~Capabilities.Deletable;
            capabilities &= ~Capabilities.Movable;


            SetPosition(new Rect(100, 200, 100, 150));

            //AddOutputPort();
        }

    }

}
