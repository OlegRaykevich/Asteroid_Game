using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MyGame
{
    class Planet: BaseObject
    {
        Image planetImage;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="dir"></param>
        /// <param name="size"></param>
        /// <param name="color">Параметр определяет цвет планеты(2 варианта)</param>
        public Planet(Point pos, Point dir, Size size, Boolean color) : base(pos, dir, size)
        {
            if(color == true)
            {
                planetImage = Image.FromFile(@"res\grayPlanet.png");
            }
            else
            {
                planetImage = Image.FromFile(@"res\redPlanet.png");
            }
        }
            

        public override void Draw()
        {
            Rectangle PlanetRectangle;
            PlanetRectangle = new Rectangle(Pos.X, Pos.Y, Size.Width, Size.Height);
            Game.Buffer.Graphics.DrawImage(planetImage, PlanetRectangle);
            
        }



        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            if (Pos.X < 0) Pos.X = Game.Width + Size.Width;
        }
    }
}
