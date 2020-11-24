using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame
{
    // 简单起见，碰撞体为方盒
    public class Collision : ICollision
    {
        private static List<Collision> collisions = new List<Collision>();
        private List<Collision> enterCollisions = new List<Collision>();
        private List<Collision> exitCollisions = new List<Collision>();
        // 相对物体中心的偏移
        public double OffsetX, OffsetY;
        public double HalfWidth, HalfHeight;
        public GameObject GameObject;
        public bool IsTrigger = false;

        public Collision(GameObject gameObject, double offsetX = 0, double offsetY = 0, 
            double halfWidth = 20, double halfHeight= 20)
        {
            GameObject = gameObject;
            OffsetX = offsetX;
            OffsetY = offsetY;
            HalfWidth = halfWidth;
            HalfHeight = halfHeight;
            collisions.Add(this);
        }

        public void Move(Vec2 dir)
        {
            GameObject.X += dir.X;
            GameObject.Y += dir.Y;
        }

        public Vec2 GetMoveDis(Vec2 moveDir)
        {
            var res = moveDir;
            for (int i = 0; i < collisions.Count; i++)
            {
                if (collisions[i] != this && Intersect(collisions[i], moveDir))
                {
                    CutDir(collisions[i], moveDir, ref res);
                }
            }

            return res;
        }

        public void Draw(Graphics gc)
        {
            var ct = GetCenter();
            gc.DrawRectangle(new Pen(Color.Red, 1), (int)(ct.X), (int)ct.Y, (int)HalfWidth * 2, 
                (int)HalfHeight * 2);
        }

        private double offset = 0;
        private void CutDir(Collision other, Vec2 dir, ref Vec2 res)
        {
            bool t1 = this.Intersect(other, new Vec2(dir.X, 0));
            bool t2 = this.Intersect(other, new Vec2(0, dir.Y));

            if (!t1 && !t2)
            {
                double x, y;
                if (dir.X > 0)
                    x = Math.Min(Math.Max(other.GetMinX() - GetMaxX() - offset, 0), res.X);
                else
                    x = Math.Max(Math.Min(other.GetMaxX() - GetMinX() + offset, 0), res.X);
                if (dir.Y > 0)
                    y = Math.Min(Math.Max(other.GetMinY() - GetMaxY() - offset, 0), res.Y);
                else
                    y = Math.Max(Math.Min(other.GetMaxY() - GetMinY() + offset, 0), res.Y);

                if (Math.Abs(x) * Math.Abs(dir.Y) > Math.Abs(y) * Math.Abs(dir.X))
                {
                    y = Math.Abs(x) * dir.Y / Math.Abs(dir.X);
                }
                else
                {
                    x = Math.Abs(y) * dir.X / Math.Abs(dir.Y);
                }

                res.X = dir.X > 0 ? Math.Min(res.X, x) : Math.Max(res.X, x);
                res.Y = dir.Y > 0 ? Math.Min(res.Y, y) : Math.Max(res.Y, y);
            }
            else
            {
                if (t1)
                {
                    if (dir.X > 0)
                        res.X = Math.Min(Math.Max(other.GetMinX() - GetMaxX() - offset, 0), res.X);
                    else if (dir.X < 0)
                        res.X = Math.Max(Math.Min(other.GetMaxX() - GetMinX() + offset, 0), res.X);
                }

                if (t2)
                {
                    if (dir.Y > 0)
                        res.Y = Math.Min(Math.Max(other.GetMinY() - GetMaxY() - offset, 0), res.Y);
                    else if (dir.Y < 0)
                        res.Y = Math.Max(Math.Min(other.GetMaxY() - GetMinY() + offset, 0), res.Y);
                }
            }
        }

        public Vec2 GetCenter()
        {
            return new Vec2(OffsetX + GameObject.X, OffsetY + GameObject.Y);
        }

        private bool Collide(Collision other)
        {
            return !(other.GetMaxX() < GetMinX() || other.GetMinX() > GetMaxX()
                || other.GetMaxY() < GetMinY() || other.GetMinY() > GetMaxY());
        }

        private bool Intersect(Collision other)
        {
            return !(other.GetMaxX() <= GetMinX() || other.GetMinX() >= GetMaxX()
                || other.GetMaxY() <= GetMinY() || other.GetMinY() >= GetMaxY());
        }

        private bool Intersect(Collision other, Vec2 dir)
        {
            OffsetX += dir.X;
            OffsetY += dir.Y;
            bool res = Intersect(other);
            OffsetX -= dir.X;
            OffsetY -= dir.Y;
            return res;
        }

        public double GetMinX()
        {
            return GameObject.X + OffsetX - HalfWidth;
        }

        public double GetMaxX()
        {
            return GameObject.X + OffsetX + HalfWidth;
        }

        public double GetMinY()
        {
            return GameObject.Y + OffsetY - HalfHeight;
        }

        public double GetMaxY()
        {
            return GameObject.Y + OffsetY + HalfHeight;
        }

        public void UpdateCollide()
        {
            exitCollisions.Clear();
            foreach (var col in enterCollisions)
            {
                exitCollisions.Add(col);
            }
            enterCollisions.Clear();

            for (int i = 0; i < collisions.Count; ++i)
            {
                if (collisions[i] != this && Collide(collisions[i]))
                {
                    enterCollisions.Add(collisions[i]);
                }
            }

            if (IsTrigger)
            {
                List<GameObject> objects = new List<GameObject>();
                foreach (var col in enterCollisions)
                {
                    objects.Add(col.GameObject);
                }
                GameObject.OnTrigger(objects);
            }
        }

        public static void Update()
        {
            for (int i = 0; i < collisions.Count; ++i)
            {
                collisions[i].UpdateCollide();
            }
        }

        public List<Collision> GetEnterCollisions()
        {
            List<Collision> res = new List<Collision>();

            return res;
        }

        ~Collision()
        {
            collisions.Remove(this);
        }
    }
}
