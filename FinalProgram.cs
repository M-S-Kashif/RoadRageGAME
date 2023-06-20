using SplashKitSDK;
using System;
using System.Collections.Generic;

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

    public class RoadRage
    {
        public RoadRage(Window gameWindow, Timer gameTimer)
        {
            _Gamewindow = gameWindow;
            _gameTimer = gameTimer;
            _Player = new Player(gameWindow);
            _BlueCar = RandomBlueCar();
            _YellowCar = RandomYellowCar();
            _Coin = RandomCoin();
        }

        private Player _Player;
        private BlueCar _BlueCar;
        private YellowCar _YellowCar;
        private Coin _Coin;
        private Window _Gamewindow;
        private Timer _gameTimer;
        private uint _points = 0;
        private uint _extrapoints;

        public bool Quit
        {
            get { return _Player.Quit; }
        }

        public void HandleInput()
        {
            SplashKit.ProcessEvents();
            _Player.HandleInput();
        }

        public void Update()
        {
            CheckAllCollisions();
            _points = UpdateTimer(_gameTimer);
            _BlueCar.Update();
            _YellowCar.Update();

            _points += _extrapoints;

            if (_Player.Quit == true)
            {
                StopTimer();
            }
        }

        public void CheckAllCollisions()
        {
            if (_Player.CollidedWith(_BlueCar))
            {
                Console.WriteLine("Watch the Road Bub!");
                _Player.LifeDeduct();
                _BlueCar = RandomBlueCar();
            }

            if (_YellowCar.IsOffscreen(_Gamewindow) || _BlueCar.IsOffscreen(_Gamewindow))
            {
                _YellowCar = RandomYellowCar();
                _BlueCar = RandomBlueCar();
            }

            if (_Player.CollideWith(_YellowCar))
            {
                Console.WriteLine("More Points!");
                _extrapoints += 10;
                _YellowCar = RandomYellowCar();
            }

            if (_Player.FindsA(_Coin))
            {
                Console.WriteLine("Extra Points!");
                //_Coin.SoundEffect();
                _extrapoints += 50;
                _Coin = RandomCoin();
            }
        }

        public uint UpdateTimer(Timer _gameTimer)
        {
            return _gameTimer.Ticks / 1000;
        }

        public void DrawTimer()
        {
            SplashKit.DrawTextOnWindow(_Gamewindow, "Points: " + _points, Color.Blue, 480, 20);
        }

        public void StopTimer()
        {
            if (_Player.Quit == true)
            {
                _gameTimer.Stop();

                Console.WriteLine("\n\n---------GAME OVER---------");
                Console.WriteLine("FINAL SCORE:   " + _points + " points");
            }
        }

        //Adds a new car...
        public BlueCar RandomBlueCar()
        {
            BlueCar _testCar = new BlueCar(_Gamewindow, _Player);
            _testCar.Draw();
            return _testCar;
        }

        public YellowCar RandomYellowCar()
        {
            YellowCar _testCar = new YellowCar(_Gamewindow);
            _testCar.Draw();
            return _testCar;
        }

        public Coin RandomCoin()
        {
            Coin _testCoin = new Coin(_Gamewindow);
            _testCoin.Draw();
            return _testCoin;
        }


        public void Draw()
        {
            _Gamewindow.Clear(Color.White);
            _BlueCar.Draw();
            _YellowCar.Draw();
            _Coin.Draw();
            _Player.Draw();
            DrawTimer();
            _Gamewindow.Refresh(60);
        }
    }

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

    public class BlueCar
    {
        public BlueCar(Window gameWindow, Player player)                                     //Constructor...
        {
            _Blue = new Bitmap("Blue Car", "Blue.png");
            BlueinWindow = gameWindow;
            if (((SplashKit.Rnd(gameWindow.Width) - Width) > 250) && (((SplashKit.Rnd(gameWindow.Width) - Width) < 500)))
            {
                X = SplashKit.Rnd(gameWindow.Width - Width);
                Y = -Height;
            }

            Quit = false;
        }

        public Window BlueinWindow;
        private Bitmap _Blue;
        private const int SPEED = 4;

        public double X { get; private set; }               //Auto-properties...
        public double Y { get; private set; }
        public bool Quit { get; private set; }
        private Vector2D Velocity { get; set; }

        public int Width                                 //Read Only Width Property for Bitmap...
        {
            get { return _Blue.Width; }
        }

        public int Height                                 //Read Only Height Property for Bitmap...
        {
            get { return _Blue.Height; }
        }

        public Circle CarHit
        {
            get { return SplashKit.CircleAt(X, Y, 50); }
        }

        public void Draw()                             //Method for drawing... 
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

        public double X { get; private set; }               //Auto-properties...
        public double Y { get; private set; }
        public bool Quit { get; private set; }

        public int Width                                 //Read Only Width Property for Bitmap...
        {
            get { return _Yellow.Width; }
        }

        public int Height                                 //Read Only Height Property for Bitmap...
        {
            get { return _Yellow.Height; }
        }

        public Circle MorePoints
        {
            get { return SplashKit.CircleAt(X, Y, 50); }
        }

        public void Draw()                             //Method for drawing... 
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
            SoundEffect gotcoin = new SoundEffect("Coins", "Coins.mp3");
            gotcoin.Play(80);
        }

        public void Draw()                             //Method for drawing... 
        {
            CoininWindow.DrawBitmap(_Coin, X, Y);
        }
    }
}
