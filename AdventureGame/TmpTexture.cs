using System;
using System.Collections.Generic;
using System.Drawing;
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
        private Bitmap image = null;
        private Bitmap showBitmap = null;

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
            if (image is null)
                return;
            if (Width != showBitmap.Width || Height != showBitmap.Height)
                Resize(Width, Height);
            gc.DrawImage(showBitmap, 
                new Point((int) (GameObject.X + offset.X), (int) (GameObject.Y + offset.Y)));
        }

        public static void DrawAllTextures(Graphics gc)
        {
            foreach (var tex in textures)
            {
                tex.Draw(gc);
            }
        }
        public void Resize(int x, int y)
        {
            if (x > image.Width || y > image.Height)
                return;
            Width = x;
            Height = y;
            var tm = new Bitmap(Width, Height);
            for (int i = 0; i < Width; i++)
            {
                int ii = i * image.Width / Width;
                for (int j = 0; j < Height; j++)
                {
                    tm.SetPixel(i, j, image.GetPixel(ii, j * image.Height / Height));
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
            return (int) GameObject.X;
        }

        private int MaxX()
        {
            return (int) (GameObject.X + Width);
        }

        private int MinY()
        {
            return (int) GameObject.Y;
        }

        private int MaxY()
        {
            return (int) (GameObject.Y + Height);
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
