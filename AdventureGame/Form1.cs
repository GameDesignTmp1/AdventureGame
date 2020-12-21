using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace AdventureGame
{
    public partial class Form1 : Form
    {
        private Player p;
        public Form1()
        {
            InitializeComponent();
            Time.Init();
            Camera.Init(this);
            Scene.DebugControl = textBox1;
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
            Scene.Update(gc);
        }

        private void 设置场景ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f2 = new SceneForm();
            timer1.Enabled = false;
            f2.Show();
        }

        private void 加载场景ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var diag = new OpenFileDialog();
            if (diag.ShowDialog() == DialogResult.OK)
            {
                timer1.Enabled = true;
                Scene.LoadScene(diag.FileName);
                Invalidate();
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ProcessKeyUp(e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }
    }
}
