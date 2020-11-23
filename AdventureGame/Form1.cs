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
        public Form1()
        {
            InitializeComponent();
            Time.Init();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ProcessKeyDown(e);
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ProcessKeyUp(e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Time.Update();
        }
    }
}
