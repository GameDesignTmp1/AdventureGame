using System;
using System.Collections.Generic;

namespace AdventureGame
{
    public class Transform : ITransform
    {
        public static List<Transform> Transforms = new List<Transform>();
        public Vec2 Location;
        public bool ToLeft = false;
        public GameObject GameObject;
        public Collision Collision;
        public bool UseGravity = false;
        public double GravityRate = 9.8;
        public Vec2 Velocity = new Vec2();
        public bool IsValid = true;
        public double GravityVelocity = 0;
        private double _gravityRate = 1;
        private Vec2 _preLoc;

        public Transform(GameObject gameObject, Vec2 location, bool turnLeft)
        {
            Location = location;
            ToLeft = turnLeft;
            GameObject = gameObject;
            Collision = gameObject.Collision;
            Transforms.Add(this);
            Location.X = gameObject.X;
            Location.Y = gameObject.Y;
            _preLoc = Location.Clone();
        }
        // 位移
        public void Translate(Vec2 dir)
        {
            var d = Collision.GetMoveDis(dir);
            Scene.DebugControl.Text = d.ToString() + " ";
            Location += d;
            Scene.DebugControl.Text += Location.ToString() + " ";
        }

        private void UpdateGravity()
        {
            if (!UseGravity)
                return;
            
            GravityVelocity += GravityRate * _gravityRate;
            var tmp = Collision.GetMoveDis(new Vec2(0, GravityVelocity) * Time.DeltaTime);
            GravityVelocity = tmp.Y / Time.DeltaTime;
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
