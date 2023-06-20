using System;
using SplashKitSDK;

namespace RoadRage
{
    public class Coin
    {
        public Coin(Window gameWindow)
        {
            _Coin = new Bitmap("Coin", "Coin.png");
            CoininWindow = gameWindow;
            X = SplashKit.Rnd(gameWindow.Width - Width);
            Y = SplashKit.Rnd(gameWindow.Height - Height);
            Quit = false;
        }

        private Window CoininWindow;
        private Bitmap _Coin;

        public double X { get; private set; }               //Auto-properties...
        public double Y { get; private set; }
        public bool Quit { get; private set; }

        public int Width                                 //Read Only Width Property for Bitmap...
        {
            get { return _Coin.Width; }
        }

        public int Height                                 //Read Only Height Property for Bitmap...
        {
            get { return _Coin.Height; }
        }

        public Circle ExtraPoints
        {
            get { return SplashKit.CircleAt(X, Y, 30); }
        }

        public void SoundEffect()                             
        {
            SoundEffect gotcoin = new SoundEffect("Coins","Coins.mp3");
            gotcoin.Play(80);
        }

        public void Draw()                             //Method for drawing... 
        {
            CoininWindow.DrawBitmap(_Coin, X, Y);
        }
    }
}
