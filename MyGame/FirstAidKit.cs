using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyGame
{
    class FirstAidKit:BaseObject
    {
        public int Power { get; set; }
        Image firstAidKitImage = Image.FromFile(@"res\fik.png");

        public FirstAidKit(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 5;
        }

        public override void Draw()
        {

            Rectangle firstAidKitRectangle = new Rectangle(Pos.X, Pos.Y, Size.Width, Size.Height);
            Game.Buffer.Graphics.DrawImage(firstAidKitImage, firstAidKitRectangle);
        }

        public override void Update()
        {
            Pos.X = Pos.X - 15;
            if (Pos.X < 0) Pos.X = Game.Width + Size.Width;
        }

        

    }
}
