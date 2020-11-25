using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame
{
    public class GameObject
    {
        public static List<GameObject> GameObjects = new List<GameObject>();
        public Collision collision = null;
        // 坐标，深度决定绘制的顺序
        public double X, Y, Depth;
        public Transform Transform = null;
        // 静态物体待定义
        public bool IsStatic = false;

        public GameObject(double x, double y, double depth = 0)
        {
            X = x;
            Y = y;
            Depth = depth;
            if (depth == 0)
            {
                collision = new Collision(this);
                Transform = new Transform(this, new Vec2(x, y), false);
            }
            GameObjects.Add(this);
        }
        // 每帧调用，更新所有物体的状态
        public static void Update(Graphics gc, bool debug = false)
        {
            foreach (var gameObject in GameObjects)
            {
                if (debug)
                    gameObject.collision.Draw(gc);

            }
        }
        // 碰撞发生时
        public virtual void OnTriggerEnter(List<GameObject> gameObjects){}
        // 碰撞体离开时
        public virtual void OnTriggerExit(List<GameObject> gameObjects){}
        // 有碰撞体驻留时
        public virtual void OnTriggerStay(List<GameObject> gameObjects){}
        ~GameObject()
        {
            GameObjects.Remove(this);
        }
    }
}
