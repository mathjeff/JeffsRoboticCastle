using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Castle.EventNodes.Customization;

namespace Castle.EventNodes.Menus
{
    abstract class SelectionEventNode<T> : EventNode
    {
        public void Show(Size screenSize)
        {
            this.screen = new SelectionScreen<T>(screenSize, this.Prompt, this.Choices);
        }
        public Screen GetScreen()
        {
            return this.screen;
        }
        public EventNode TimerTick(double numSeconds)
        {
            if (this.screen.Chosen)
            {
                this.OnChoice(this.screen.Choice);
                this.screen = null;
                return this.GetNextNode();
            }
            return this;
        }

        public ChoiceBuilder<T> Choices;
        public String Prompt;
        
        public abstract void OnChoice(T choice);
        public abstract EventNode GetNextNode();

        SelectionScreen<T> screen;
    }
}
