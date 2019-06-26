using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MyGame
{
    class Asteroids:BaseObject, ICloneable, IComparable
    {
        public int Power { get; set; } = 3;
        Image asteroidImage = Image.FromFile(@"res\comet.png");

        public Asteroids(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 1;
        }

        public object Clone()
        {
            Asteroids asteroid = new Asteroids(new Point(Pos.X, Pos.Y), new Point(Dir.X, Dir.Y), new Size(Size.Width, Size.Height))
            { Power = Power };
            return asteroid;
        }

        public override void Draw()
        {
            
            Rectangle asteroidRectangle = new Rectangle(Pos.X, Pos.Y, Size.Width, Size.Height);
            Game.Buffer.Graphics.DrawImage(asteroidImage, asteroidRectangle);
        }

        public override void Update()
        {
            Pos.X = Pos.X - 15;
            if (Pos.X < 0) Pos.X = Game.Width + Size.Width;
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj is Asteroids temp)
            {
                if (Power > temp.Power)
                    return 1;
                if (Power < temp.Power)
                    return -1;
                else
                    return 0;
            }
            throw new ArgumentException("Parameter is not а Asteroid!");
        }
    }
}
