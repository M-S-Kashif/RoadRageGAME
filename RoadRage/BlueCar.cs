using System;
using SplashKitSDK;

namespace RoadRage
{
    public class BlueCar
    {
        public BlueCar(Window gameWindow, Player player)                                     
        {
            _Blue = new Bitmap("Blue Car", "Blue.png");
            BlueinWindow = gameWindow;
            if ( ((SplashKit.Rnd(gameWindow.Width) - Width) > 250 ) && (((SplashKit.Rnd(gameWindow.Width) - Width) < 500)))
            {
                X = SplashKit.Rnd(gameWindow.Width - Width);
                Y = -Height;
            }
            
            Quit = false;
        }

        public Window BlueinWindow;
        private Bitmap _Blue;
        private const int SPEED = 4;

        public double X { get; private set; }               
        public double Y { get; private set; }
        public bool Quit { get; private set; }
        private Vector2D Velocity { get; set; }

        public int Width                                
        {
            get { return _Blue.Width; }
        }

        public int Height                                 
        {
            get { return _Blue.Height; }
        }

        public Circle CarHit
        {
            get { return SplashKit.CircleAt(X, Y, 50); }
        }

        public void Draw()                              
        {
            BlueinWindow.DrawBitmap(_Blue, X, Y);
        }

        public void Update()
        {
            if (IsOffscreen(BlueinWindow))
            {
                if (((SplashKit.Rnd(BlueinWindow.Width) - Width) > 250) && (((SplashKit.Rnd(BlueinWindow.Width) - Width) < 500)))
                {
                    X = SplashKit.Rnd(BlueinWindow.Width - Width);
                }
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
