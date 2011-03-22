using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

class AudioPlayer
{
// public
    public AudioPlayer(Canvas newCanvas)
    {
        this.canvas = newCanvas;
        this.player = new MediaElement();
        //this.player.Source = new Uri("09WhoAmILivingFor_.m4a", UriKind.Relative);
        //this.player.LoadedBehavior = MediaState.Play;
        //this.player.Volume = 50;
        newCanvas.Children.Add(this.player);
    }
    public void setSoundFile(String path)
    {
        //return;
        this.player.Source = new Uri(path, UriKind.Relative);
        this.player.LoadedBehavior = MediaState.Manual;
        //this.player.LoadedBehavior = MediaState.Play;
    }
    public void play()
    {
        //return;
        //this.player.IsMuted = false;
        //this.player.Volume = 1;
        this.player.Play();
    }
    public void stop()
    {
        this.player.Stop();
    }
// private
    //String soundPath;
    MediaElement player;
    Canvas canvas;
};