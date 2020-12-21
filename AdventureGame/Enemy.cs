using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdventureGame
{
    public enum State
    {
        Idle,
        Walk,
        Chase,
        Attack,
    }
    public class Enemy : GameObject
    {
        public double AttackDis = 100;
        public double WarnDis = 150;
        public double ChaseDis = 200;
        public State State = State.Idle;
        public double Speed = 50;
        public Collision AttackCollision = null;

        private Random random = new Random();
        private double _nextTime = 0;
        private double _attackInterval = 2;
        private double _attackTime = 0;
        private bool _toLeft = false;
        public Enemy(double x, double y, double depth = 0, string tag = "") : base(x, y, depth, tag)
        {
            Transform.UseGravity = true;
            AttackCollision = new Collision(this);
            AttackCollision.IsRigid = false;
            AttackCollision.IsTrigger = false;
            Animations = new Dictionary<string, TmpAnimation>();
        }

        public override void Update(Graphics gc)
        {
            base.Update(gc);
            UpdateState();
            Act();
        }

        public void Act()
        {
            switch (State)
            {
                case State.Attack:
                    Attack();
                    break;
                case State.Chase:
                    MoveToPlayer();
                    break;
                case State.Idle:
                    Animations["Idle"].Play();
                    break;
                case State.Walk:
                    Walk();
                    break;
            }
        }

        void Attack()
        {
            TurnToPlayer();
            if (_attackTime <= 0)
            {
                AttackCollision.IsTrigger = true;
                if (Animations.ContainsKey("Attack"))
                    Animations["Attack"].Play();
            }
            else if (_attackTime > _attackInterval / 2)
            {
                AttackCollision.IsTrigger = false;
                if (Animations.ContainsKey("Attack"))
                    Animations["Attack"].Reset();
            }
            else if (_attackTime > _attackInterval)
                _attackTime = 0;
            _attackTime += Time.DeltaTime;
        }

        void Walk()
        {
            foreach (var anim in Animations)
            {
                foreach (var texture in anim.Value.Animations)
                {
                    texture.Turn(_toLeft);
                }
            }

            Transform.Translate(new Vec2(Speed * (_toLeft ? -1 : 1) * Time.DeltaTime, 0));
        }
        void MoveToPlayer()
        {
            TurnToPlayer();
            if (Scene.Player.X < X)
                Transform.Translate(new Vec2(-Speed * Time.DeltaTime, 0));
            else
                Transform.Translate(new Vec2(Speed * Time.DeltaTime, 0));
        }
        public void UpdateState()
        {
            if (CanAttack())
                State = State.Attack;
            else if (IsPlayerNear())
                State = State.Chase;
            else if (!CanChase())
                DecideWalk();
            else
                State = State.Chase;
        }
        private void DecideWalk()
        {
            if (_nextTime <= 0)
            {
                if (random.Next(2) == 0)
                {
                    if (random.Next(2) == 1)
                        _toLeft = true;
                    else
                        _toLeft = false;
                    State = State.Walk;
                }
                else
                    State = State.Idle;
                _nextTime = (random).NextDouble() + 0.5;
            }

            _nextTime -= Time.DeltaTime;
        }
        private void TurnToPlayer()
        {
            foreach (var anim in Animations)
            {
                foreach (var texture in anim.Value.Animations)
                {
                    texture.Turn(Scene.Player.X < X);
                }
            }
        }
        private double PlayerDis()
        {
            return Math.Abs(Scene.Player.X - X);
        }
        private bool IsPlayerNear()
        {
            return PlayerDis() <= WarnDis;
        }
        private bool CanChase()
        {
            return PlayerDis() <= ChaseDis;
        }
        private bool CanAttack()
        {
            return PlayerDis() <= AttackDis || _attackTime < _attackInterval;
        }
    }
}
