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
    public partial class ScaneForm : Form
    {
        private int _x, _y;
        private bool _grab;
        private Point _point;
        private int _len = 3000;
        private double _scale = 1;
        private List<string> _pictureList = new List<string>();
        public ScaneForm()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var pen = new Pen(Color.Red, 2);
            Graphics gc = e.Graphics;
            int xx = (int)(_x * _scale);
            int yy = (int) (_y * _scale);
            int len = (int)(_len * _scale);
            gc.DrawLine(pen, xx, yy, xx + len, yy);
            gc.DrawLine(pen, xx, yy, xx - len, yy);
            gc.DrawLine(pen, xx, yy, xx, yy + len);
            gc.DrawLine(pen, xx, yy, xx, yy - len);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (e.Delta > 0)
            {
                if (_scale < 1.8)
                    _scale += 0.1;
            }
            else if (e.Delta < 0)
            {
                if (_scale > 0.2)
                    _scale -= 0.1;
            }
            else
                return;
            Invalidate();
        }

        private void ScaneForm_SizeChanged(object sender, EventArgs e)
        {
            panel2.Width = (int)(Width * 0.2);
            panel2.Height = Height - 100;
            panel2.Location = new Point((int)(Width * 0.78), 0);
        }

        private void ScaneForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                _grab = false;
        }

        private void ScaneForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (_grab)
            {
                _x += (e.X - _point.X);
                _y += (e.Y - _point.Y);
                _point = e.Location;
                Invalidate();
            }
        }

        private void ScaneForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _point = e.Location;
                _grab = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var diag = new OpenFileDialog();
            if (diag.ShowDialog() == DialogResult.OK)
            {
                var names = diag.FileNames;
                foreach (var name in names)
                {
                    _pictureList.Add(name);
                }
            }
            comboBox1.Items.Clear();
            foreach (var name in _pictureList)
            {
                var t = name.Split('\\');
                comboBox1.Items.Add(t[t.Length - 1]);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var idx = comboBox1.SelectedIndex;
            var source = new Bitmap(_pictureList[idx]);
            Bitmap target = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            for (int i = 0; i < pictureBox1.Width; ++i)
            {
                for (int j = 0; j < pictureBox1.Height; ++j)
                {
                    target.SetPixel(i, j,
                        source.GetPixel((int) (i * source.Width / pictureBox1.Width), 
                        (int)(j * source.Height / pictureBox1.Height)));
                }
            }
            source.SetResolution(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = target;
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void ScaneForm_Load(object sender, EventArgs e)
        {
            Width = 1200;
            Height = 600;
            _x = Width / 2;
            _y = Height / 2;
        }
    }
}
