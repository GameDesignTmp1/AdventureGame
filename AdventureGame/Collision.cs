using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame
{
    class Collision : ICollision
    {
        private static List<Collision> collisions = new List<Collision>();
        public int X, Y;
        public int Width, Height;
        public GameObject GameObject;
        public Collision(GameObject gameObject, int x = 0, int y = 0, int width = 10, int height = 10)
        {
            GameObject = gameObject;
            X = x;
            Y = y;
            width = Width;
            height = Height;
            collisions.Add(this);
        }
        public Vec3 GetMoveDis(Vec3 moveDir)
        {
            for (int i = 0; i < collisions.Count; i++)
            {
                if (collisions[i] != this)
                {

                }
            }
            throw new NotImplementedException();
        }

        public bool IsCollide(Collision collision)
        {
            throw new NotImplementedException();
        }

        ~Collision()
        {
            collisions.Remove(this);
        }
    }
}
