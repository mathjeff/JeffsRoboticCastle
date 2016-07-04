using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

// Embodies a node in the tree that is the storyline
// Could be a level-selection screen, a level, a cutscene, etc
// The reason for the existence of both the EventNode and the Screen class is to facilitate lazy-loading
// This allows us to create a plan for which levels will exist without preloading the images for each screen
namespace Castle.EventNodes
{
    interface EventNode
    {
        void Show(Size screenSize);
        Screen GetScreen();
        EventNode TimerTick(double numSeconds);
    }
}
