using SplashKitSDK;
using System;

namespace RoadRage
{
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

            if (_YellowCar.IsOffscreen(_Gamewindow) || _BlueCar.IsOffscreen(_Gamewindow) )
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
            if(_Player.Quit == true)
            {
                _gameTimer.Stop();

                Console.WriteLine("\n\n---------GAME OVER---------");
                Console.WriteLine("FINAL SCORE:   " + _points + " points");
            }
        }

        //Adds a new car...
        public BlueCar RandomBlueCar()
        {
            BlueCar _testCar = new BlueCar(_Gamewindow,_Player);
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
}
