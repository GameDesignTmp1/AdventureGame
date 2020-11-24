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
        private GameObject o1, o2, o3;
        public Form1()
        {
            InitializeComponent();
            Time.Init();
            o1 = new GameObject(0, 0);
            o2 = new GameObject(39, 45);
            o3 = new GameObject(-10, 60);
            o2.Transform.UseGravity = false;
            o3.Transform.UseGravity = false;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ProcessKeyDown(e);
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        //private double left = 0, up = 0;
        private double speed = 10;
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics gc = e.Graphics;
            GameObject.Update(gc, true);
            double left = 0, up = 0;
            if (Input.IsKeyPress(Keys.A))
                left -= 0.1;
            if (Input.IsKeyPress(Keys.D))
                left += 0.1;
            if (Input.IsKeyPress(Keys.W))
                up -= 0.1;
            if (Input.IsKeyPress(Keys.S))
                up += 0.1;
            if (left == 0 && up == 0)
                return;
            left *= speed;
            up *= speed;
            o2.Transform.Translate(new Vec2(left, up));
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ProcessKeyUp(e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Time.Update();
            Collision.Update();
            Transform.Update();

            this.Invalidate();
        }
    }
}
