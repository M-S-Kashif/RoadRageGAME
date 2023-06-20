using System;
using SplashKitSDK;

namespace RoadRage
{
    public class YellowCar
    {
        public YellowCar(Window gameWindow)
        {
            _Yellow = new Bitmap("Yellow Car", "Yellow.png");
            YellowinWindow = gameWindow;

            if (((SplashKit.Rnd(gameWindow.Width)) > 200) && (((SplashKit.Rnd(gameWindow.Width)) < 420)))
            {
                X = SplashKit.Rnd(gameWindow.Width - Width);
                Y = -Height;
            }

            Quit = false;
        }

        private Window YellowinWindow;
        public Bitmap _Yellow;
        private const int SPEED = 5;

        public double X { get; private set; }              
        public double Y { get; private set; }
        public bool Quit { get; private set; }

        public int Width                                 
        {
            get { return _Yellow.Width; }
        }

        public int Height                                 
        {
            get { return _Yellow.Height; }
        }

        public Circle MorePoints
        {
            get { return SplashKit.CircleAt(X, Y, 50); }
        }

        public void Draw()                              
        {
            YellowinWindow.DrawBitmap(_Yellow, X, Y);
        }

        public void Update()
        {
            if (IsOffscreen(YellowinWindow))
            {
                X = SplashKit.Rnd(YellowinWindow.Width - Width);
            }
            Y += SPEED;
        }

        public bool IsOffscreen(Window screen)
        {
            if (X < -Width || X > screen.Width || Y < -Height || Y > screen.Height)
            {
                return true;
            }
            return false;
        }
    }
}