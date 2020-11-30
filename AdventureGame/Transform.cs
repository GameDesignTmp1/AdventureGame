using System.Collections.Generic;

namespace AdventureGame
{
    public class Transform : ITransform
    {
        public static List<Transform> Transforms = new List<Transform>();
        public Vec2 Location;
        private Vec2 _preLoc;
        public bool ToLeft = false;
        public GameObject GameObject;
        public Collision Collision;
        public bool UseGravity = false;
        public double Gravity = 9.8;
        public Vec2 Velocity = new Vec2();
        private double _gravityRate = 10;
        public bool IsValid = true;

        public Transform(GameObject gameObject, Vec2 location, bool turnLeft)
        {
            Location = location;
            ToLeft = turnLeft;
            GameObject = gameObject;
            Collision = gameObject.collision;
            Transforms.Add(this);
            Location.X = gameObject.X;
            Location.Y = gameObject.Y;
            _preLoc = Location.Clone();
        }
        // 位移
        public void Translate(Vec2 dir)
        {
            var d = Collision.GetMoveDis(dir);
            Location += d;
        }

        private void UpdateGravity()
        {
            if (!UseGravity)
                return;
            
            Velocity.Y += Gravity * _gravityRate * Time.DeltaTime;
            var tmp = Collision.GetMoveDis(Velocity * Time.DeltaTime);

            Location += tmp;
        }

        public void UpdateTransform()
        {
            UpdateGravity();

            Velocity = (Location - _preLoc) / Time.DeltaTime;

            GameObject.X = Location.X;
            GameObject.Y = Location.Y;
            _preLoc = Location;
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
            Collision = null;
        }
        ~Transform()
        {
            Destroy();
        }
    }
}
