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
        private Vec2 _preLoc = new Vec2();
        public bool ToLeft = false;
        public GameObject GameObject;
        public Collision collision;
        public bool UseGravity = false;
        public double Gravity = 9.8;
        public Vec2 Velocity = new Vec2();
        private double _gravityRate = 10;

        public Transform(GameObject gameObject, Vec2 location, bool turnLeft)
        {
            Location = location;
            ToLeft = turnLeft;
            GameObject = gameObject;
            collision = gameObject.collision;
            Transforms.Add(this);
            Location.X = gameObject.X;
            Location.Y = gameObject.Y;
            _preLoc = Location.Clone();
        }
        // 位移
        public void Translate(Vec2 dir)
        {
            var d = collision.GetMoveDis(dir);
            Velocity += d;
        }

        private void UpdateGravity()
        {
            if (!UseGravity)
                return;
            
            Velocity.Y += Gravity * _gravityRate * Time.DeltaTime;
            var tmp = collision.GetMoveDis(Velocity * Time.DeltaTime);
            if (tmp.Y == 0)
                Velocity.Reset();

            Location += tmp;
        }

        public void UpdateTransform()
        {
            _preLoc = Location;
            UpdateGravity();

            Velocity = (Location - _preLoc) / Time.DeltaTime;

            GameObject.X = Location.X;
            GameObject.Y = Location.Y;
        }

        public static void Update()
        {
            foreach (var transform in Transforms)
            {
                transform.UpdateTransform();
            }
        }

        public Vec2 GetVelocity()
        {
            return Velocity;
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

        public void Destroy()
        {
            GameObject = null;
            Transforms.Remove(this);
            collision = null;
        }
        ~Transform()
        {
            Destroy();
        }
    }
}
