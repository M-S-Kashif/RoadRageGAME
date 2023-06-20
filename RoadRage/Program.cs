using SplashKitSDK;
using System;

namespace RoadRage
{
    class Program
    {
        public static void Main()
        {
            Window gameWindow = new Window("Robot Dodge", 600, 750);
            Timer gametimer = new Timer("Game Timer");
            RoadRage play = new RoadRage(gameWindow, gametimer);
            gametimer.Start();
            while (play.Quit == false)
            {
                SplashKit.ProcessEvents();
                play.HandleInput();
                play.Update();
                play.Draw();
            }
        }
    }
}
