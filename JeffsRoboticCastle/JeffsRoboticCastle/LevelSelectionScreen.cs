using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

/*
class LevelSelectionScreen : Screen
{
// public
    public LevelSelectionScreen()
    {
    }
    public override void Initialize(Point screenLocation, Size screenSize)
    {
        this.setBackgroundImage(ImageLoader.loadImage("LevelSelectionImage.png", screenSize));
        this.addSubviews();
    }
    private int parseInt(TextBox field)
    {
        if (field.Text == "")
            field.Text = "0";
        return Int32.Parse(field.Text);
    }
    public int getChosenLevelNumber()
    {
        return parseInt(levelBox);
    }
    public override void KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        // intercept and override the key events so that a keypress doesn't exit the menu
    }
    public override void KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
    {
    }
    // private
    void addSubviews()
    {
        
        //System.Windows.Controls.ListView
        Label levelLabel = new Label();
        levelLabel.Content = "Level (1-9):";
        this.addControl(levelLabel, 140, 100, 100, 25);
        this.levelBox = new TextBox();
        levelBox.Text = "1";
        this.addControl(levelBox, getLeft(levelLabel), getBottom(levelLabel), 100, 20);
        Button doneButton = new Button();
        doneButton.Content = "Done";
        doneButton.Click += new System.Windows.RoutedEventHandler(requestToExit);
        this.addControl(doneButton, getLeft(levelBox), getBottom(levelBox), 100, 20);
    }

    System.Windows.Controls.TextBox levelBox;
}
*/