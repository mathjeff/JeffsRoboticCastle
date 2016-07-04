using Castle.EventNodes.Customization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Castle.EventNodes.Menus
{
    class SelectionScreen<T> : Screen
    {
        public SelectionScreen(Size screenSize, String prompt, IEnumerable<T> choices)
            : base(screenSize)
        {
            this.initialize(screenSize, prompt, new ChoiceBuilder<T>(choices).Build());
        }

        public SelectionScreen(Size screenSize, String prompt, Dictionary<String, T> choices) : base(screenSize)
        {
            this.initialize(screenSize, prompt, choices.AsEnumerable());
        }
        public SelectionScreen(Size screenSize, String prompt, ChoiceBuilder<T> choices)
            : base(screenSize)
        {
            this.initialize(screenSize, prompt, choices.Build());
        }
        private void initialize(Size screenSize, String prompt, IEnumerable<KeyValuePair<String, T>> choices)
        {
            // TODO this logic should use the VisiPlacer git project for more configurability
            // I just haven't necessarily implemented everything in VisiPlacer that we might want in this project
            int numItems = choices.Count() + 1;
            double heightPerItem = screenSize.Height / numItems;
            double widthPerItem = screenSize.Width;
            TextBlock promptBlock = new TextBlock();
            promptBlock.Text = prompt;
            promptBlock.FontSize = 24;
            promptBlock.TextWrapping = TextWrapping.Wrap;
            this.addControl(promptBlock, 0, 0, widthPerItem, heightPerItem);
            double y = heightPerItem;
            this.choicesByButton = new Dictionary<Button, T>();
            foreach (KeyValuePair<String, T> choiceEntry in choices)
            {
                T choice = choiceEntry.Value;
                Button button = new Button();
                button.Content = choiceEntry.Key;
                button.Click += new RoutedEventHandler(this.ButtonClick);
                this.addControl(button, 0, y, widthPerItem, heightPerItem);
                y += heightPerItem;
                this.choicesByButton[button] = choice;
            }
        }

        public void ButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            T choice = this.choicesByButton[button];
            this.Choice = choice;
            this.Chosen = true;
        }
        
        public T Choice;
        public bool Chosen;
        private Dictionary<Button, T> choicesByButton;

        
    }
}