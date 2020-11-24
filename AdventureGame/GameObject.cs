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

        public double X, Y, Depth;
        public Transform Transform = null;
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

        public static void Update(Graphics gc, bool debug = false)
        {
            foreach (var gameObject in GameObjects)
            {
                if (debug)
                    gameObject.collision.Draw(gc);
            }
        }

        public virtual void OnTrigger(List<GameObject> gameObjects)
        {
        }

        ~GameObject()
        {
            GameObjects.Remove(this);
        }
    }
}
