using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdventureGame
{
    public class Player : GameObject
    {
        public double Speed = 80;
        public double JumpSpeed = 250;
        private bool _jump = false;
        public Player(double x, double y, double depth = 0, string tag = "Player") : base(x, y, depth, tag)
        {
            Transform.UseGravity = true;
            Animations = new Dictionary<string, TmpAnimation>();
            Texture.IsValid = false;
        }

        public override void OnTriggerEnter(List<GameObject> gameObjects)
        {
            base.OnTriggerEnter(gameObjects);

        }

        public override void Update(Graphics gc)
        {
            base.Update(gc);
            Move();
            if (_jump && Transform.GetVelocity().Y == 0)
                _jump = false;
            if (!_jump && Input.IsKeyPress(Keys.Space))
                Jump();
            Vec2 offset = new Vec2(Camera.GetCenter());
            offset = -offset;
        }

        void Move()
        {
            double right = 0;
            if (Input.IsKeyPress(Keys.A))
                right -= 1;
            if (Input.IsKeyPress(Keys.D))
                right += 1;
            if (right != 0)
            {
                if (Animations.ContainsKey("Walk"))
                    Animations["Walk"].Play();
                Turn(right < 0);
                Transform.Translate(new Vec2(right, 0) * Speed * Time.DeltaTime);
            }
            else if (Animations.ContainsKey("Idle"))
                Animations["Idle"].Play();
        }
        void Turn(bool left)
        {
            foreach (var anim in Animations)
            {
                foreach (var texture in anim.Value.Animations)
                {
                    texture.Turn(left);
                }
            }
        }
        private void Jump()
        {
            _jump = true;
            Transform.GravityVelocity -= JumpSpeed;
            if (Animations.ContainsKey("Jump"))
                Animations["Jump"].IsPlay = true;
        }
    }
}
