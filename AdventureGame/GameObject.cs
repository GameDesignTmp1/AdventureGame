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

        public int X, Y;
        private Collision collision;
        GameObject(int x, int y)
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
