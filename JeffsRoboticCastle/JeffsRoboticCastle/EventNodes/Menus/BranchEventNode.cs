using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Castle.EventNodes.Menus
{
    class BranchEventNode : SelectionEventNode<EventNode>
    {
        public BranchEventNode(String prompt)
        {
            this.Prompt = prompt;
            this.Choices = new Customization.ChoiceBuilder<EventNode>();
        }
        public void AddChoice(String label, EventNode node)
        {
            this.Choices = this.Choices.And(label, node);
        }
        public override void OnChoice(EventNode choice)
        {
            this.nextNode = choice;
        }
        public override EventNode GetNextNode()
        {
            return this.nextNode;
        }
        private EventNode nextNode;
    }
}
