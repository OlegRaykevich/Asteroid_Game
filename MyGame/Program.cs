using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MyGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Form form = new Form
            {
                Width = 1000,
                Height = 800,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                StartPosition = FormStartPosition.CenterScreen
            };

            Game.Init(form);


            
            form.Show();
            Game.Draw();
            Application.Run(form);
        }
    }
}
