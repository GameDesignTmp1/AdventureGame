using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame
{
    public class TmpTexture : ITexture
    {
        public static List<TmpTexture> textures = new List<TmpTexture>();
        public GameObject GameObject;
        public int Width, Height;
        public bool IsValid = true;
        private bool _left = false;
        private Bitmap image = null;
        private Bitmap showBitmap = null;
        private int _w, _h; // 临时大小

        public TmpTexture(GameObject gameObject)
        {
            GameObject = gameObject;
            textures.Add(this);
        }
        public void LoadTexture(string filename)
        {
            image = new Bitmap(filename);
            showBitmap = new Bitmap(filename);
            Width = image.Width;
            Height = image.Height;
        }

        public void Draw(Graphics gc, double scale = 1)
        {
            if (image is null)
                return;
            Draw(gc, new Vec2(), scale);
        }

        public void Draw(Graphics gc, Vec2 offset, double scale = 1)
        {
            if (image is null || !IsValid)
                return;
            if (offset.X != 0 || offset.Y != 0)
                ;
            gc.DrawImage(showBitmap,
                new Point((int) (GameObject.X + offset.X) - showBitmap.Width / 2,
                    (int) (GameObject.Y + offset.Y) - showBitmap.Height / 2));
        }

        public void Turn(bool left)
        {
            if (left != _left)
            {
                var t = new Bitmap(showBitmap.Width, showBitmap.Height);
                for (int i = 0; i < showBitmap.Width; i++)
                {
                    for (int j = 0; j < showBitmap.Height; j++)
                    {
                        t.SetPixel(i, j, 
                            showBitmap.GetPixel(showBitmap.Width - i - 1, j));
                    }
                }

                showBitmap = t;
                _left = left;
            }
        }
        public void Resize(int x, int y)
        {
            Width = x;
            Height = y;
            var tm = new Bitmap(Width, Height);
            for (int i = 0; i < Width; i++)
            {
                int ii = i * image.Width / Width % image.Width;
                for (int j = 0; j < Height; j++)
                {
                    tm.SetPixel(i, j, 
                        image.GetPixel(ii, j * image.Height / Height % image.Height));
                }
            }

            showBitmap = tm;
        }

        public void TmpResize(double scale)
        {
            _w = (int) (Width * scale);
            _h = (int) (Height * scale);
            if (_w >= image.Width || _h >= image.Height)
                return;

            var tm = new Bitmap(_w, _h);
            for (int i = 0; i < _w; i++)
            {
                int ii = i * image.Width / _w % image.Width;
                for (int j = 0; j < _h; j++)
                {
                    var color = image.GetPixel(ii, j * image.Height / _h % image.Height);
                    tm.SetPixel(i, j, color);
                }
            }

            showBitmap = tm;
        }

        public bool Detect(Point pos)
        {
            return pos.X > MinX() && pos.X < MaxX() && pos.Y > MinY() && pos.Y < MaxY();
        }

        private int MinX()
        {
            return (int) GameObject.X - showBitmap.Width / 2;
        }

        private int MaxX()
        {
            return (int) (GameObject.X + Width / 2);
        }

        private int MinY()
        {
            return (int) GameObject.Y - showBitmap.Height / 2;
        }

        private int MaxY()
        {
            return (int) (GameObject.Y + Height / 2);
        }

        public void Destroy()
        {
            GameObject = null;
            textures.Remove(this);
        }

        ~TmpTexture()
        {
            Destroy();
        }
    }
}
