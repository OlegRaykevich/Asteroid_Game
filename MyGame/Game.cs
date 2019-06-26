using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MyGame
{
    delegate void GetData(string msg);

    /// <summary>
    /// Класс Game содержит в себе логику игру.
    /// </summary>
    class Game
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;

        private static List<Asteroids> _asteroids;
        public static Stars[] _star;
        public static Planet[] _grayPlanet;
        public static Planet[] _redPlanet;
        private static Ship _ship;
        private static FirstAidKit[] _fik;

        private static List<Bullet> _bullets = new List<Bullet>();

        private static int AsteroidsCount = 1;
        private static int AsteroidsCountIncrement = 2;

        public static int Width { get; set; }
        public static int Height { get; set; }
        private const int GameWindowMaxSize = 1000;

        static Game()
        {
            _bullets = new List<Bullet>();
            _asteroids = new List<Asteroids>(5);
        }

        private static Timer _timer = new Timer();
        public static Random Rnd = new Random();

        private static void ValidateGameWindowSize(int width, int height)
        {
            if (width > GameWindowMaxSize || width < 0 || height > GameWindowMaxSize || height < 0)
                throw new ArgumentOutOfRangeException($"Значения ширины / высоты игрового поля должны находиться в диапазоне [0,{GameWindowMaxSize}]");
        }

        /// <summary>
        /// Метод Init формирует графический кадр.
        /// </summary>
        /// <param name="form"></param>
        public static void Init(Form form)
        {
            Graphics g;
            form.KeyDown += Form_KeyDown;
            _context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();

            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;
            ValidateGameWindowSize(Width, Height);

            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));


            _timer.Interval = 45;
            _timer.Start();
            _timer.Tick += Timer_Tick;


            Load();
            Ship.MessageDie += Finish;
            Bullet.MessageBulletDestroyed += Bullet.ShowMessageBulletDestroyed;
            Bullet.MessageBulletCreated += Bullet.ShowMessageBulletCreated;
            Ship.LooseEnergy += Ship.ShowMessageShipLooseEnergy;
            Ship.AddEnergy += Ship.ShowMessageShipAddEnergy;
            Ship.AddScore += Ship.ShowMessageShipAddScore;

        }

        /// <summary>
        /// Мето воспроизводит один такт таймера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

        /// <summary>
        /// Метед Draw отрисовывает кадр.
        /// </summary>
        public static void Draw()
        {
            Buffer.Graphics.Clear(Color.Black);
            
            
            foreach (BaseObject obj in _star)
                obj.Draw();
            foreach (BaseObject obj in _grayPlanet)
                obj.Draw();
            foreach (BaseObject obj in _redPlanet)
                obj.Draw();
            foreach (BaseObject obj in _asteroids)
                obj?.Draw();
            foreach (BaseObject obj in _fik)
                obj.Draw();
            foreach (Bullet b in _bullets)
                b.Draw();

            _ship?.Draw();

            if (_ship != null)
            {
                Buffer.Graphics.DrawString("Energy:" + _ship.Energy, SystemFonts.DefaultFont, Brushes.White, 0, 0);
                Buffer.Graphics.DrawString("Score:" + _ship.Score, SystemFonts.DefaultFont, Brushes.White, 60, 0);
            }
                

            Buffer.Render();
        }

        /// <summary>
        /// Изменяет состояние объектов
        /// </summary>
        public static void Update()
        {



            foreach (Bullet b in _bullets) b?.Update();


            foreach (BaseObject obj in _star)
                obj.Update();
            foreach (BaseObject obj in _grayPlanet)
                obj.Update();
            foreach (BaseObject obj in _redPlanet)
                obj.Update();



            for (var i = 0; i < _asteroids.Count; i++)
            {
                if (AsteroidsCount == 0)
                {
                    GenerateAsteroid(_asteroids.Count + AsteroidsCountIncrement);
                    AsteroidsCount = _asteroids.Count;
                }
                if (_asteroids[i] == null) continue;
                _asteroids[i].Update();
                for (var j = 0; j < _bullets.Count; j++)
                {
                    if (_asteroids[i] != null && _bullets[j].Collision(_asteroids[i]))
                    {
                        System.Media.SystemSounds.Hand.Play();
                        _asteroids[i] = null;
                        _bullets.RemoveAt(j);
                        _ship?.EnergyIncrease();
                        j--;
                        AsteroidsCount--;
                    }
                }
                if (_asteroids[i] == null || !_ship.Collision(_asteroids[i])) continue;
                {
                    var rnd = new Random();
                    _ship?.EnergyLow(rnd.Next(10, 20));
                    System.Media.SystemSounds.Asterisk.Play();
                    _asteroids[i] = null;
                    AsteroidsCount--;
                }
                if (_ship.Energy <= 0) _ship?.Die();
            }

            for (var i = 0; i < _fik.Length; i++)
            {
                _fik[i].Update();
                if (!_ship.Collision(_fik[i])) continue;
                if (_ship.Energy >= 100)
                {
                    break;
                }
                else
                {
                    _ship?.EnergyIncrease();
                    System.Media.SystemSounds.Exclamation.Play();
                }
            }
        }

        /// <summary>
        /// Метод инициализирует объекты
        /// </summary>
        public static void Load()
        {

            GenerateAsteroid(AsteroidsCount);
            _star = new Stars[30];
            _grayPlanet = new Planet[1];
            _redPlanet = new Planet[1];

            _bullets.Add(new Bullet(new Point(-20, -20), new Point(25, 0), new Size(25, 10)));

            _ship = new Ship(new Point(10, 400), new Point(5, 5), new Size(30, 30));
            _fik = new FirstAidKit[5];

            var rnd = new Random();

            


            for (int i = 0; i < _fik.Length; i++)
            {
                int r = rnd.Next(5, 50);
                _fik[i] = new FirstAidKit(new Point(1500, rnd.Next(0, Game.Height)), new Point(-r, r), new Size(25, 25));
            }

            for (var i = 0; i < _star.Length; i++)
            {
                int r = rnd.Next(5, 50);
                _star[i] = new Stars(new Point(1000, rnd.Next(0, Game.Height)), new Point(-r, r), new Size(25, 25));
            }

            for (int i = 0; i < _grayPlanet.Length; i++)
                _grayPlanet[i] = new Planet(new Point(700, 200), new Point(-5, 0), new Size(150, 150), true);
            for (int i = 0; i < _redPlanet.Length; i++)
                _redPlanet[i] = new Planet(new Point(200, 600), new Point(-1, 0), new Size(50, 50), false);

            

        }


        /// <summary>
        /// Реализация управления космическим кораблем с клавиатуры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                _bullets.Add(new Bullet(new Point(_ship.Rect.X + 10, _ship.Rect.Y + 4), new Point(4, 0), new Size(12, 7)));
                _bullets[_bullets.Count - 1].MessageCreate();
            }
            if (e.KeyCode == Keys.Up) _ship.Up();
            if (e.KeyCode == Keys.Down) _ship.Down();
        }

        /// <summary>
        /// Генерация сообщения о конце игры
        /// </summary>
        public static void Finish()
        {
            _timer.Stop();
            Buffer.Graphics.DrawString("The End", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.White, 200, 100);
            Buffer.Render();
        }





        public static void GenerateAsteroid(int Count)
        {
            _asteroids.Clear();
            var rnd = new Random();
            for (var i = 0; i < Count; i++)
            {
                int r = rnd.Next(20, 50);
                _asteroids.Add(new Asteroids(new Point(Game.Width, rnd.Next(1, Game.Height + 25)), new Point(-r / 5, r), new Size(r, r)));
            }
        }

        private static void WriteGameMessage(string message)
        {
            SizeF MessageSize = Buffer.Graphics.MeasureString(message, new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline));
            Buffer.Graphics.DrawString(message, new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.White, (Width - MessageSize.Width) / 2, Height / 2);
            Buffer.Render();
        }

    }
}
