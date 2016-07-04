using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Asks the player to make a selection, and then moves to the next node regardless of the choice
namespace Castle.EventNodes.Menus
{
    abstract class LinearSelectionEventNode<T> : SelectionEventNode<T>
    {
        public EventNode NextNode;
        public override EventNode GetNextNode() { return this.NextNode; }
    }
}
