using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

// Contains a group of EventNode objects, to facilitate reuse of a group in several places
namespace Castle.EventNodes
{
    class CompositeEventNode
    {
        public void Show(Size size)
        {
            this.childEvent = this.StartEvent;
            this.childEvent.Show(size);
        }
        public Screen GetScreen()
        {
            return this.childEvent.GetScreen();
        }
        public EventNode timerTick(double numSeconds)
        {
            return this.childEvent.TimerTick(numSeconds);
        }
        public EventNode StartEvent;
        private Screen screen;
        private EventNode childEvent;
    }
}
