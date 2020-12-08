using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdventureGame
{
    public class GameObject
    {
        private class Sorter : IComparer<GameObject>
        {
            public int Compare(GameObject x, GameObject y)
            {
                if (x.Depth > y.Depth)
                    return 1;
                else if (x.Depth < y.Depth)
                    return -1;
                return 0;
            }
        }
        public static List<GameObject> GameObjects = new List<GameObject>();
        public Collision Collision = null;
        public Music Music = null;
        public Transform Transform = null;
        public TmpTexture Texture = null;
        // 坐标，深度决定绘制的顺序
        public double X, Y;
        public double Depth;
        // 静态物体待定义
        public bool IsStatic = false;
        public string Tag;
        public bool IsValid = true;

        public GameObject(double x, double y, double depth = 0, string tag = "")
        {
            X = x;
            Y = y;
            Depth = depth;
            Tag = tag;
            if (depth == 0)
            {
                Collision = new Collision(this);
                Transform = new Transform(this, new Vec2(x, y), false);
            }
            Texture = new TmpTexture(this);
            GameObjects.Add(this);
            GameObjects.Sort(new Sorter());
        }

        // 每帧调用，更新所有物体的状态
        public static void Update(Graphics gc, bool debug = false)
        {
            Vec2 offset = new Vec2(Camera.GetCenter());
            offset = -offset;
            foreach (var gameObject in GameObjects)
            {
                gameObject.Texture.Draw(gc, offset);
                if (debug)
                    gameObject.Collision.Draw(gc, offset);
                gameObject.Transform.UpdateTransform();
                gameObject.Update();
            }
        }
        // 碰撞发生时
        public virtual void OnTriggerEnter(List<GameObject> gameObjects){}
        // 碰撞体离开时
        public virtual void OnTriggerExit(List<GameObject> gameObjects){}
        // 有碰撞体驻留时
        public virtual void OnTriggerStay(List<GameObject> gameObjects){}
        // 每帧更新逻辑
        public virtual void Update()
        {
        }

        public void Destroy()
        {
            IsValid = false;
            GameObjects.Remove(this);
            if (Transform != null)
                Transform.Destroy();
            Transform = null;
            if (Collision != null)
                Collision.Destroy();
            Collision = null;
        }
        ~GameObject()
        {
            Destroy();
        }
    }
}
