using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

class LevelSelectionScreen : MenuScreen
{
// public
    public LevelSelectionScreen(Canvas c, double[] screenSize)
    {
        this.initialize(c, screenSize);
    }
    public override void initialize(System.Windows.Controls.Canvas c, double[] screenSize)
    {
        base.initialize(c, screenSize);
        this.setBackgroundBitmap(ImageLoader.loadImage("LevelSelectionImage.png"));
        this.addSubviews();
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
        /*
        Label levelLabel = new Label();
        levelLabel.Content = "Choose a level to start at. Level 1 is recommended.";
        this.addControl(levelLabel, 100, 100, 100, 20);
        */
        //System.Windows.Controls.ListView
        Label levelLabel = new Label();
        levelLabel.Content = "Level (1-5):";
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