using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyGame
{
    class Ship:BaseObject
    {
        public static event Message MessageDie;

        public static event Message LooseEnergy;
        public static event Message AddEnergy;
        public static event Message AddScore;

        Image shipImage = Image.FromFile(@"res\spaceship.png");

        private int _energy = 100;
        public int Energy => _energy;

        private int _score = 0;
        public int Score { get => _score; set => _score = value; }

        public Ship(Point pos, Point dir,Size size):base(pos, dir, size)
        {

        }

        /// <summary>
        /// Уменьшение энергии после удара астероида
        /// </summary>
        /// <param name="n"></param>
        public void EnergyLow(int n)
        {
            _energy -= n;
        }

        /// <summary>
        /// Восстановление энергии при помощи аптечек(шестеренок)
        /// </summary>
        public void EnergyIncrease()
        {
            _energy += 5;
        }

        public override void Draw()
        {
            Rectangle shipRectangle = new Rectangle(Pos.X, Pos.Y, Size.Width, Size.Height);
            Game.Buffer.Graphics.DrawImage(shipImage, shipRectangle);
        }

        public override void Update()
        {
        }

        public void Up()
        {
            if (Pos.Y > 0) Pos.Y = Pos.Y - Dir.Y;
        }

        public void Down()
        {
            if (Pos.Y < Game.Height) Pos.Y = Pos.Y + Dir.Y;
        }

        /// <summary>
        /// Уничтожение корабля
        /// </summary>
        public void Die()
        {
            MessageDie?.Invoke();
            Console.WriteLine("Корабль разрушен!");
        }

        public static void ShowMessageShipLooseEnergy()
        {
            Console.WriteLine("Корабль потерял энергию!");
        }

        public static void ShowMessageShipAddEnergy()
        {
            Console.WriteLine("Корабль пополнил энергию!");
        }

        public static void ShowMessageShipAddScore()
        {
            Console.WriteLine("Счет увеличен!");
        }

    }
}
