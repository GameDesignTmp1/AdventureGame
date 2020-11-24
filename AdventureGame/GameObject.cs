using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame
{
    public class GameObject
    {
        static List<GameObject> gameObjects = new List<GameObject>();

        public double X, Y;
        private Collision collision;

        public GameObject(double x, double y)
        {
            X = x;
            Y = y;
            gameObjects.Add(this);
        }

        ~GameObject()
        {
            gameObjects.Remove(this);
        }
    }
}
