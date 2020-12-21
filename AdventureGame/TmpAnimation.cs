using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame
{
    public class TmpAnimation : IAnimation
    {
        public static List<TmpAnimation> Anims = new List<TmpAnimation>();
        public List<TmpTexture> Animations = new List<TmpTexture>();
        public GameObject GameObject;
        public bool IsPlay = true;
        private int _playRate = 1;
        private double _cnt = 0;
        public TmpAnimation(GameObject obj, string texFolder)
        {
            GameObject = obj;
            LoadAnimation(texFolder);
            Anims.Add(this);
        }
        public void LoadAnimation(string texFolder)
        {
            var files = Directory.GetFiles(texFolder);
            foreach (var file in files)
            {
                TmpTexture tex = null;
                try
                {
                    tex = new TmpTexture(GameObject);
                    tex.LoadTexture(file);
                }
                catch (Exception e)
                {
                    continue;
                }
                Animations.Add(tex);
            }
        }
        public void Draw(Graphics gc, Vec2 offset)
        {
            if (!IsPlay || Animations.Count <= 0)
                return;
            _cnt += Time.DeltaTime;
            Animations[(int)(_cnt * _playRate) % Animations.Count].Draw(gc, offset);
        }

        public void Init()
        {
            foreach (var tex in Animations)
            {
                tex.Resize(GameObject.Texture.Width, GameObject.Texture.Height);
            }
        }
        public static void DrawAll(Graphics gc, Vec2 offset)
        {
            foreach (var anim in Anims)
            {
                anim.Draw(gc, offset);
            }
        }
        public void Pause()
        {
            IsPlay = false;
        }
        public void Play()
        {
            IsPlay = true;
        }
        public void Reset()
        {
            _cnt = 0;
        }

        public void Destroy()
        {
            GameObject = null;
            foreach (var tex in Animations)
            {
                tex.Destroy();
            }

            Anims.Remove(this);
        }
    }
}
