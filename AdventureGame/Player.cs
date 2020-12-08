using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdventureGame
{
    public class Player : GameObject
    {
        public Player(double x, double y, double depth = 0, string tag = "Player") : base(x, y, depth, tag)
        {
            Transform.UseGravity = true;
        }

        public override void OnTriggerEnter(List<GameObject> gameObjects)
        {
            base.OnTriggerEnter(gameObjects);
        }

        public override void Update()
        {
            base.Update();
            double up = 0, right = 0;
            if (Input.IsKeyPress(Keys.S))
                up += 0.1;
            if (Input.IsKeyPress(Keys.W))
                up -= 0.1;
            if (Input.IsKeyPress(Keys.A))
                right -= 0.1;
            if (Input.IsKeyPress(Keys.D))
                right += 0.1;
            if (right != 0 || up != 0)
                Transform.Translate(new Vec2(right, up) * 10);
        }
    }
}
