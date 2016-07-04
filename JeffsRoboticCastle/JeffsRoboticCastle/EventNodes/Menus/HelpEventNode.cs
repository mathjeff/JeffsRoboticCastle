using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Offers to display help to the user
namespace Castle.EventNodes.Menus
{
    class HelpEventNode : BranchEventNode
    {
        public HelpEventNode() : base("Do you know how to play? The instructions are short but are only available here.")
        {
            InfoEventNode music = InfoEventNode.FromImage("Music.png");

            InfoEventNode controls = InfoEventNode.FromImage("Controls.png");
            music.NextNode = controls;

            InfoEventNode rules = InfoEventNode.FromText("This game is primarily about collecting and using weapon augments."
                + " Each weapon augment can be attached to a weapon to improve its stats."
                + " Weapon augments can be reassigned to other weapons during shop events."
                + " You may also be able to trade weapon augments."
                + " All weapons you receive will be the same before you add augments"
                + " to them."
                );
            controls.NextNode = rules;
            
            rules.NextNode = this;

            this.AddChoice("Help!", music);
        }
        public EventNode NextNode
        {
            set
            {
                this.AddChoice("Play!", value);
            }
        }

    }
}
