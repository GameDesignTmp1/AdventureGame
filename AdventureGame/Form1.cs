using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdventureGame
{
    public partial class Form1 : Form
    {
        private GameObject o1, o2;
        private Collision co1, co2;
        public Form1()
        {
            InitializeComponent();
            Time.Init();
            o1 = new GameObject(0, 0);
            o2 = new GameObject(39, 45);

            co1 = new Collision(o1, 0, 0);
            co2 = new Collision(o2,
                0, 0);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ProcessKeyDown(e);
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        //private double left = 0, up = 0;
        private double speed = 3;
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics gc = e.Graphics;
            co1.Draw(gc);
            co2.Draw(gc);
            double left = 0, up = 0;
            if (Input.IsKeyPress(Keys.A))
                left -= 0.1;
            if (Input.IsKeyPress(Keys.D))
                left += 0.1;
            if (Input.IsKeyPress(Keys.W))
                up -= 0.1;
            if (Input.IsKeyPress(Keys.S))
                up += 0.1;
            left *= 3;
            up *= 3;
            if (left == 0 && up == 0)
                return;
            var res = co2.GetMoveDis(new Vec2(left, up));
            co2.GameObject.X += res.X;
            co2.GameObject.Y += res.Y;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ProcessKeyUp(e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Time.Update();

            this.Invalidate();
        }
    }
}
