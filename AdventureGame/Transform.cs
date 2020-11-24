using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame
{
    public class Transform : ITransform
    {
        public static List<Transform> Transforms = new List<Transform>();
        public Vec2 Location = new Vec2();
        public bool ToLeft = false;
        public GameObject GameObject;
        public Collision collision;
        public bool UseGravity = true;
        public double Gravity = 9.8;
        private double _gravityRate = 0.1;
        public Transform(GameObject gameObject)
        {
            GameObject = gameObject;
            collision = gameObject.collision;
            Transforms.Add(this);
        }

        public Transform(GameObject gameObject, Vec2 location, bool turnLeft)
        {
            Location = location;
            ToLeft = turnLeft;
            GameObject = gameObject;
            collision = gameObject.collision;
            Transforms.Add(this);
        }
        // 位移
        public void Translate(Vec2 dir)
        {
            var d = collision.GetMoveDis(dir);
            GameObject.X += d.X;
            GameObject.Y += d.Y;
        }

        public void UpdateGravity()
        {
            var dir = collision.GetMoveDis(new Vec2(0, Gravity * _gravityRate));
            GameObject.Y += dir.Y;
        }

        public static void Update()
        {
            foreach (var transform in Transforms)
            {
                if (transform.UseGravity)
                    transform.UpdateGravity();
            }
        }
        // 转向
        public void TurnLeft()
        {
            ToLeft = true;
        }
        public void TurnRight()
        {
            ToLeft = false;
        }

        ~Transform()
        {
            Transforms.Remove(this);
        }
    }
}
