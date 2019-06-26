using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MyGame
{
    class Bullet:BaseObject
    {
        public static event Message MessageBulletDestroyed;
        public static event Message MessageBulletCreated;

        Image bulletImage = Image.FromFile(@"res\Green_laser.png");
        public Bullet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {

        }

        public override void Draw()
        {
            Rectangle bulletRectangle = new Rectangle(Pos.X, Pos.Y, Size.Width, Size.Height);
            Game.Buffer.Graphics.DrawImage(bulletImage, bulletRectangle);
        }

        public override void Update()
        {
            Pos.X = Pos.X + 15;
        }

        public static void ShowMessageBulletDestroyed()
        {
            Console.WriteLine("Пуля уничтожена!");
        }

        public void MessageDestroyed()
        {
            MessageBulletDestroyed?.Invoke();
        }

        public void MessageCreate()
        {
            MessageBulletCreated?.Invoke();
        }
        public static void ShowMessageBulletCreated()
        {
            Console.WriteLine("Выстрел!");
        }

    }
}
