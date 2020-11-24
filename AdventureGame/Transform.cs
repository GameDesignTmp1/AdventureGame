using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame
{
    public class Transform : ITransform
    {
        public Vec2 Location = new Vec2();
        public bool ToLeft = false;
        public GameObject GameObject;
        public Collision collision;

        public Transform(GameObject gameObject)
        {
            GameObject = gameObject;
            collision = gameObject.collision;
        }

        public Transform(GameObject gameObject, Vec2 location, bool turnLeft)
        {
            Location = location;
            ToLeft = turnLeft;
            GameObject = gameObject;
            collision = gameObject.collision;
        }

        public void Translate(Vec2 dir)
        {
            var d = collision.GetMoveDis(dir);
            GameObject.X += d.X;
            GameObject.Y += d.Y;
        }

        public void TurnLeft()
        {
            ToLeft = true;
        }

        public void TurnRight()
        {
            ToLeft = false;
        }
    }
}
