using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MyGame
{
    class Stars:BaseObject
    {
        Image starImage;

        public Stars(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            starImage = Image.FromFile(@"res\star.png");
        }

        public override void Draw()
        {
            Rectangle starRectangle = new Rectangle(Pos.X, Pos.Y, Size.Width, Size.Height);
            Game.Buffer.Graphics.DrawImage(starImage, starRectangle);
        }

        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            if (Pos.X < 0) Pos.X = Game.Width + Size.Width;
        }
    }
}
