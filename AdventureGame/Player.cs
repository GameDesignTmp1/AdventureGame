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
        public double Speed = 200;
        private bool _jump = false;
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
            double right = 0;
            if (Input.IsKeyPress(Keys.A))
                right -= 0.1;
            if (Input.IsKeyPress(Keys.D))
                right += 0.1;
            if (right != 0)
                Transform.Translate(new Vec2(right, 0) * Speed * Time.DeltaTime);
            if (_jump && Transform.GetVelocity().Y == 0)
                _jump = false;
            if (!_jump && Input.IsKeyPress(Keys.Space))
                Jump();
        }

        private void Jump()
        {
            _jump = true;
            Transform.GravityVelocity -= Speed;
        }
    }
}
