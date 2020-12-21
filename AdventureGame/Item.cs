using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame
{
    public class Item : GameObject
    {
        public Item(double x, double y, double depth = 0, string tag = "") : base(x, y, depth, tag)
        {
            Collision.IsRigid = false;
        }

        public override void OnTriggerEnter(List<GameObject> gameObjects)
        {
            base.OnTriggerEnter(gameObjects);
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.Tag == "Player")
                {
                    Scene.DebugControl.Text = "Player enter";
                    Destroy();
                    break;
                }
            }
        }

        public override void OnTriggerExit(List<GameObject> gameObjects)
        {
            base.OnTriggerExit(gameObjects);
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.Tag == "Player")
                {
                    Scene.DebugControl.Text = "Player exit";
                    break;
                }
            }
        }
    }
}
