using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace AdventureGame
{
    public partial class SceneForm : Form
    {
        private int _x, _y; // 原点位置
        private bool _grabRight, _grabLeft;
        private Point _point;
        private int _len = 3000;
        private double _scale = 1;
        private List<string> _pictureList = new List<string>();
        private List<Tuple<GameObject, string>> objects = new List<Tuple<GameObject, string>>();
        private GameObject selectedGameObject = null;
        private bool _gen = false;
        private int _align = 0;
        public SceneForm()
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
            foreach (var tuple in objects)
            {
                var obj = tuple.Item1;
                double dx = (obj.X - _x) * _scale + xx, dy = (obj.Y - _y) * _scale + yy;
                var p = new Vec2(dx - obj.X, dy - obj.Y);
                obj.Texture.Draw(gc, p);
                obj.Collision.Draw(gc, p, _scale);
            }
        }
        // 缩放
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            return;
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

            foreach (var tuple in objects)
            {
                tuple.Item1.Texture.TmpResize(_scale);
            }
            Invalidate();
        }

        private void ScaneForm_SizeChanged(object sender, EventArgs e)
        {
            panel2.Width = (int)(Width * 0.2);
            panel2.Height = Height - 100;
            panel2.Location = new Point((int)(Width * 0.78), 50);
        }

        private void ScaneForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                _grabRight = false;
            else if (e.Button == MouseButtons.Left)
                _grabLeft = false;
        }

        private void ScaneForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (_grabRight)
            {
                int dx = e.X - _point.X;
                int dy = e.Y - _point.Y;
                foreach (var tuple in objects)
                {
                    tuple.Item1.Transform.Location += new Vec2(dx, dy);
                }
                _x += dx;
                _y += dy;
                Invalidate();
            }

            if (_grabLeft)
            {
                int dx = e.X - _point.X;
                int dy = e.Y - _point.Y;
                selectedGameObject.Transform.Location += new Vec2(dx, dy);
                Invalidate();
            }

            if (_gen)
            {
                selectedGameObject.Transform.Location = new Vec2(e.X, e.Y);
                Invalidate();
            }
            _point = e.Location;
        }

        private void ScaneForm_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    _point = e.Location;
                    _grabRight = true;
                    break;
                case MouseButtons.Left:
                    if (_gen)
                        _gen = false;
                    else
                    {
                        foreach (var tuple in objects)
                        {
                            if (tuple.Item1.Texture.Detect(e.Location))
                            {
                                selectedGameObject = tuple.Item1;
                                _grabLeft = true;
                                UpdateSelectedInfo();
                                break;
                            }
                        }
                    }
                    break;
            }
        }
        // 更新显示信息
        private void UpdateSelectedInfo()
        {
            if (selectedGameObject == null)
                return;
            BoxX.Text = selectedGameObject.X.ToString();
            BoxY.Text = selectedGameObject.Y.ToString();
            BoxDepth.Text = selectedGameObject.Depth.ToString();
            BoxImgHeight.Text = selectedGameObject.Texture.Height.ToString();
            BoxImgWidth.Text = selectedGameObject.Texture.Width.ToString();
            BoxTag.Text = selectedGameObject.Tag;
            BoxOffsetX.Text = selectedGameObject.Collision.OffsetX.ToString();
            BoxOffsetY.Text = selectedGameObject.Collision.OffsetY.ToString();
            BoxHalfHeight.Text = selectedGameObject.Collision.HalfHeight.ToString();
            BoxHalfWidth.Text = selectedGameObject.Collision.HalfWidth.ToString();
        }
        // 根据当前贴图生成物体
        private void button1_Click(object sender, EventArgs e)
        {
            if (_pictureList.Count <= 0)
                return;
            selectedGameObject = new GameObject(0, 0);
            var idx = comboBox1.SelectedIndex;
            selectedGameObject.Texture.LoadTexture(_pictureList[idx]);
            selectedGameObject.Texture.Resize(50, 50);
            objects.Add(new Tuple<GameObject, string>(selectedGameObject, _pictureList[idx]));
            _gen = true;
        }
        // 更新贴图列表
        private void UpdatePicList()
        {
            comboBox1.Items.Clear();
            foreach (var name in _pictureList)
            {
                var t = name.Split('\\');
                comboBox1.Items.Add(t[t.Length - 1]);
            }
        }
        // 添加贴图
        private void button3_Click(object sender, EventArgs e)
        {
            var diag = new OpenFileDialog();
            if (diag.ShowDialog() == DialogResult.OK)
            {
                var names = diag.FileNames;
                foreach (var name in names)
                {
                    if (!_pictureList.Contains(name))
                        _pictureList.Add(name);
                }
            }
            UpdatePicList();
        }
        // 选择贴图
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var idx = comboBox1.SelectedIndex;
            if (idx >= _pictureList.Count)
                return;
            Bitmap source = null;
            try
            {
                source = new Bitmap(_pictureList[idx]);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                MessageBox.Show("图片格式错误");
                return;
            }

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
        // 场景保存格式
        public struct Saver
        {
            public double X, Y, OffsetX, OffsetY, Depth;
            public double HalfWidth, HalfHeight;
            public int Width, Height;
            public string Tag, Filename;
        }
        // 保存场景
        private void button4_Click(object sender, EventArgs e)
        {
            var diag = new SaveFileDialog();
            if (diag.ShowDialog() == DialogResult.OK)
            {
                List<Saver> js = new List<Saver>();
                foreach (var tp in objects)
                {
                    Object tmp = new Saver();
                    var obj = tp.Item1;
                    ObjCopy(tmp, obj);
                    ObjCopy(tmp, obj.Collision);
                    ObjCopy(tmp, obj.Texture);
                    Saver t = (Saver) tmp;
                    t.Filename = tp.Item2;
                    t.X -= _x;
                    t.Y -= _y;
                    js.Add(t);
                }
                File.WriteAllText(diag.FileName, JsonConvert.SerializeObject(js));
            }
        }

        private void SceneForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (var tuple in objects)
            {
                tuple.Item1.Destroy();
            }
        }
        // 加载已有场景
        private void button5_Click(object sender, EventArgs e)
        {
            var diag = new OpenFileDialog();
            if (diag.ShowDialog() == DialogResult.OK)
            {
                byte[] str = new byte[10240];
                int length = diag.OpenFile().Read(str, 0, 10240);
                string s = Encoding.Default.GetString(str);
                List<Saver> tmpList;
                try
                {
                    tmpList = JsonConvert.DeserializeObject<List<Saver>>(s);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("场景文件加载错误，必须是合法的 Json 文件且不能有中文");
                    return;
                }
                List<Saver> js = new List<Saver>();
                foreach (var tuple in objects)
                {
                    tuple.Item1.Destroy();
                }
                objects.Clear();
                foreach (var tp in tmpList)
                {
                    var obj = new GameObject(tp.X + _x, tp.Y + _y, tp.Depth);
                    obj.Texture.LoadTexture(tp.Filename);
                    ObjCopy(obj, tp);
                    ObjCopy(obj.Collision, tp);
                    ObjCopy(obj.Texture, tp);
                    obj.Texture.Resize(obj.Texture.Width, obj.Texture.Height);
                    objects.Add(new Tuple<GameObject, string>(
                        obj, tp.Filename));
                    if (!_pictureList.Contains(tp.Filename))
                        _pictureList.Add(tp.Filename);
                }
                UpdatePicList();
                if (_pictureList.Count > 0)
                    comboBox1.SelectedIndex = 0;
                Invalidate();
            }
        }

        private void SceneForm_Load(object sender, EventArgs e)
        {
            Width = 1200;
            Height = 800;
            _x = Width / 2;
            _y = Height / 2;
        }

        private void 赋值_Click(object sender, EventArgs e)
        {
            if (selectedGameObject is null)
                return;
            selectedGameObject.Transform.Location.X = double.Parse(BoxX.Text);
            selectedGameObject.Transform.Location.Y = double.Parse(BoxY.Text);
            selectedGameObject.Collision.HalfHeight = double.Parse(BoxHalfHeight.Text);
            selectedGameObject.Collision.HalfWidth = double.Parse(BoxHalfWidth.Text);
            selectedGameObject.Depth = double.Parse(BoxDepth.Text);
            selectedGameObject.Collision.OffsetX = double.Parse(BoxOffsetX.Text);
            selectedGameObject.Collision.OffsetY = double.Parse(BoxOffsetY.Text);
            selectedGameObject.Texture.Resize(int.Parse(BoxImgWidth.Text),
                int.Parse(BoxImgHeight.Text));
            selectedGameObject.Tag = BoxTag.Text;
        }

        private void 显示_Click(object sender, EventArgs e)
        {
            UpdateSelectedInfo();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (var tuple in objects)
            {
                tuple.Item1.Transform.UpdateTransform();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

        }
        // 销毁物体
        private void button2_Click(object sender, EventArgs e)
        {
            if (selectedGameObject is null)
                return;
            int idx;
            for (idx = 0;idx < objects.Count;++idx)
                if (objects[idx].Item1 == selectedGameObject)
                    break;
            objects.RemoveAt(idx);
            selectedGameObject.Destroy();
        }

        private void 刷新_Click(object sender, EventArgs e)
        {
            Invalidate();
        }

        public static void ObjCopy(Object target, Object src)
        {
            if (target is null || src is null)
                return;
            var info1 = target.GetType().GetFields();
            var info2 = src.GetType().GetFields();
            foreach (var targetInfo in info1)
            {
                foreach (var srcInfo in info2)
                {
                    if (srcInfo.Name == targetInfo.Name)
                    {
                        targetInfo.SetValue(target, srcInfo.GetValue(src));
                        break;
                    }
                }
            }
        }
    }
}
