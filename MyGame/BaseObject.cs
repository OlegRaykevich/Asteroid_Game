using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MyGame
{
    /// <summary>
    /// Базовый класс для всех объектов фона.
    /// </summary>
    abstract class BaseObject:ICollision
    {
        public delegate void Message();

        protected Point Pos;
        protected Point Dir;
        protected Size Size;


        protected BaseObject(Point pos, Point dir, Size size)
        {
            Pos = pos;
            Dir = dir;
            Size = size;

        }
        /// <summary>
        /// Метод рисует объект
        /// </summary>
        abstract public void Draw();

        /// <summary>
        /// Метод изменяет состояние объекта(перемещает его на плоскости)
        /// </summary>
        abstract public void Update();
            
        
        public bool Collision(ICollision o) => o.Rect.IntersectsWith(this.Rect);
        public Rectangle Rect => new Rectangle(Pos, Size);

    }
}
