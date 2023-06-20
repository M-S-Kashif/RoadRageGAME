using SplashKitSDK;
using System;
using System.Collections.Generic;

namespace RoadRage
{
    public class Player
    {
        public Player(Window gameWindow)                                    
        {
            _PlayerBitmap = new Bitmap("Red Car", "Player.png");
            _gameWindow = gameWindow;
            _Lifebar = new List<Bitmap>();
            AddLives(lives);
            X = (_gameWindow.Width - Width) / 2;
            Y = (_gameWindow.Height - Height) / 2;
            Quit = false;
        }

        private Window _gameWindow;
        private Bitmap _PlayerBitmap;
        private int lives = 5;
        private List<Bitmap> _Lifebar;
        private Bitmap _LifeBitmap;

        public double X { get; private set; }               
        public double Y { get; private set; }
        public bool Quit { get; private set; }

        public int Width                                 
        {
            get { return _PlayerBitmap.Width; }
        }

        public int Height                                 
        {
            get { return _PlayerBitmap.Height; }
        }

        public void Draw()                             
        {
            _gameWindow.DrawBitmap(_PlayerBitmap, X, Y);
            DrawLives(lives);
        }

        public void AddLives(int num)
        {
            for (int i = 0; i < num; i++)
            {
                _LifeBitmap = new Bitmap("LifeBitmap", "life.png");      
                _Lifebar.Add(_LifeBitmap);
            }
        }

        public void DrawLives(int num)
        {
            const double corner = 20;
            double displayGap = 20;

            foreach (Bitmap life in _Lifebar)
            {
                _gameWindow.DrawBitmap(life, displayGap, corner);
                displayGap += 50;
            }
        }

        public void LifeDeduct()
        {
            if (lives != 0)
            {
                _LifeBitmap = _Lifebar[--lives];
                _Lifebar.Remove(_LifeBitmap);
            }
            else
            {
                Console.WriteLine("------ YOU DIED!-----");
                Quit = true;
            }
        }

        public void Move(double xval, double yval)
        {
            X = X + xval;
            Y = Y + yval;
        }

        public void HandleInput()
        {
            SplashKit.ProcessEvents();
            if (SplashKit.KeyDown(KeyCode.UpKey)) //Move Up
            {
                Move(0, -5);
                Console.WriteLine("Up...");
                StayOnWindow(_gameWindow);
            }

            if (SplashKit.KeyDown(KeyCode.DownKey))  //Move Down
            {
                Move(0, 5);
                Console.WriteLine("Down...");
                StayOnWindow(_gameWindow);
            }

            if (SplashKit.KeyDown(KeyCode.RightKey))  //Move Right
            {
                Move(5, 0);
                Console.WriteLine("Right...");
                StayOnWindow(_gameWindow);
            }

            if (SplashKit.KeyDown(KeyCode.LeftKey))  //Move Left
            {
                Move(-5, 0);
                Console.WriteLine("Left...");
                StayOnWindow(_gameWindow);
            }

            if (SplashKit.KeyDown(KeyCode.EscapeKey)) //Quit the Game
            {
                Quit = true;
                Console.WriteLine("Quitting the Game...");
            }
        }

        public void StayOnWindow(Window gameWindow)
        {
            const int GAP = 10;
            //Checking horizontal Borders....
            if (X < GAP)
            {
                X = GAP;
                Console.WriteLine("Left Border Touched!");
            }
            if (X > gameWindow.Width - (GAP + _PlayerBitmap.Width))
            {
                X = gameWindow.Width - (GAP + _PlayerBitmap.Width);
                Console.WriteLine("Right Border Touched!");
            }

            //Checking vertical Borders....
            if (Y < GAP)
            {
                Y = GAP;
                Console.WriteLine("Top Border Touched!");
            }
            if (Y > gameWindow.Height - (GAP + _PlayerBitmap.Height))
            {
                Y = gameWindow.Height - (GAP + _PlayerBitmap.Height);
                Console.WriteLine("Bottom Border Touched!");
            }
        }

        public bool CollidedWith(BlueCar blue)
        {
            return _PlayerBitmap.CircleCollision(X, Y, blue.CarHit);
        }

        public bool CollideWith(YellowCar yellow)
        {
            return _PlayerBitmap.CircleCollision(X, Y, yellow.MorePoints);
        }

        public bool FindsA(Coin coin)
        {
            return _PlayerBitmap.CircleCollision(X, Y, coin.ExtraPoints);
        }
    }
}




