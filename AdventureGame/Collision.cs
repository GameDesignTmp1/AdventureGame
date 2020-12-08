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
        private static readonly List<Collision> Collisions = new List<Collision>();
        private readonly List<Collision> _enterCollisions = new List<Collision>();
        private readonly List<Collision> _exitCollisions = new List<Collision>();
        private List<Collision> _stayCollisions = new List<Collision>();
        // 相对物体中心的偏移
        public double OffsetX = 0, OffsetY = 0;
        public double HalfWidth = 20, HalfHeight = 20;
        public GameObject GameObject;
        public bool IsTrigger = false;
        // 被设置为刚体的物体能够阻止其他物体前进
        public bool IsRigid = true;
        public bool IsValid = true;

        public Collision(GameObject gameObject)
        {
            GameObject = gameObject;
            Collisions.Add(this);
        }
        /**
         * 遍历所有碰撞体
         * @moveDir 碰撞体将要移动的方向
         *
         * 返回：碰撞体在这个方向能够移动的距离（二维）
         */
        public Vec2 GetMoveDis(Vec2 moveDir)
        {
            var res = moveDir;
            for (int i = 0; i < Collisions.Count; i++)
            {
                // 对方是刚体才能够阻止其他物体前进
                if (Collisions[i] != this && Collisions[i].IsValid && Collisions[i].IsRigid &&
                    Intersect(Collisions[i], moveDir))
                {
                    CutDir(Collisions[i], moveDir, ref res);
                }
            }

            return res;
        }
        // 绘制碰撞盒子，用于调试
        public void Draw(Graphics gc)
        {
            var ct = GetCenter();
            gc.DrawRectangle(new Pen(Color.Red, 1), (int)(ct.X), (int)ct.Y, (int)HalfWidth * 2, 
                (int)HalfHeight * 2);
        }
        public void Draw(Graphics gc, Vec2 offset, double scale = 1)
        {
            var ct = GetCorner();
            gc.DrawRectangle(new Pen(Color.Red, 1), 
                (int)(ct.X + offset.X), (int) (ct.Y + offset.Y), 
                (int) (scale * HalfWidth * 2),
                (int) (scale * HalfHeight * 2));
        }
        // 偏移，用于调整碰撞限度
        private double offset = 0;
        /*
         * 将碰撞盒根据四边分成八个区域，对角区域特殊处理
         * @other   将要碰撞的物体
         * @dir     移动方向
         * @res     结果（即 dir 方向能够移动的距离（二维）
         */
        private void CutDir(Collision other, Vec2 dir, ref Vec2 res)
        {
            bool t1 = this.Intersect(other, new Vec2(dir.X, 0));
            bool t2 = this.Intersect(other, new Vec2(0, dir.Y));
            // 如果单独往 X、Y 方向移动都不会碰撞，说明在对角区域
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
                // 相当于发射一条射线，将盒体移动到射线与另一个盒体相交的地方
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
            // 当盒体处于上下左右的区域时，直接获得最大距离
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

        private Vec2 GetCorner()
        {
            return new Vec2(OffsetX + GameObject.X - HalfWidth,
                OffsetY + GameObject.Y - HalfHeight);
        }
        private Vec2 GetCenter()
        {
            return new Vec2(OffsetX + GameObject.X + HalfWidth, 
                OffsetY + GameObject.Y + HalfHeight);
        }
        // 判断是否碰撞，边缘重合也算
        private bool Collide(Collision other)
        {
            return other.IsTrigger && !(other.GetMaxX() < GetMinX() || other.GetMinX() > GetMaxX()
                || other.GetMaxY() < GetMinY() || other.GetMinY() > GetMaxY());
        }
        // 判断是否重合，边缘重合不算，这样做的目的是为了使两个物体能够产生紧贴的状态
        // 如此 Collide 函数才能够生效
        private bool Intersect(Collision other)
        {
            return !(other.GetMaxX() <= GetMinX() || other.GetMinX() >= GetMaxX()
                || other.GetMaxY() <= GetMinY() || other.GetMinY() >= GetMaxY());
        }
        // 移动 dir 后是否重合
        private bool Intersect(Collision other, Vec2 dir)
        {
            OffsetX += dir.X;
            OffsetY += dir.Y;
            bool res = Intersect(other);
            OffsetX -= dir.X;
            OffsetY -= dir.Y;
            return res;
        }

        private double GetMinX()
        {
            return GameObject.X + OffsetX - HalfWidth;
        }

        private double GetMaxX()
        {
            return GameObject.X + OffsetX + HalfWidth;
        }

        private double GetMinY()
        {
            return GameObject.Y + OffsetY - HalfHeight;
        }

        private double GetMaxY()
        {
            return GameObject.Y + OffsetY + HalfHeight;
        }
        // 更新这一帧碰到的碰撞体，上一帧仍然留着的碰撞体，这一帧离开的碰撞体，顺便调用物体的触发函数
        private void UpdateCollide()
        {
            _exitCollisions.Clear();
            foreach (var col in _stayCollisions)
            {
                if (!col.IsValid)
                    _stayCollisions.Remove(col);
                else if (!Collide(col))
                {
                    _exitCollisions.Add(col);
                    _stayCollisions.Remove(col);
                }
            }
            foreach (var col in _enterCollisions)
            {
                if (!Collide(col) && col.IsValid)
                    _exitCollisions.Add(col);
                else
                    _stayCollisions.Add(col);
            }
            _enterCollisions.Clear();

            for (int i = 0; i < Collisions.Count; ++i)
            {
                if (Collisions[i] != this && Collisions[i].IsValid && Collide(Collisions[i]))
                {
                    _enterCollisions.Add(Collisions[i]);
                }
            }
            // 如果可以触发，调用物体的三个触发函数
            if (IsTrigger)
            {
                if (_enterCollisions.Count > 0)
                {
                    List<GameObject> objects = new List<GameObject>();
                    foreach (var col in _enterCollisions)
                    {
                        objects.Add(col.GameObject);
                    }

                    GameObject.OnTriggerEnter(objects);
                }

                if (_exitCollisions.Count > 0)
                {
                    List<GameObject> objects = new List<GameObject>();
                    foreach (var col in _exitCollisions)
                    {
                        objects.Add(col.GameObject);
                    }

                    GameObject.OnTriggerExit(objects);
                }

                if (_stayCollisions.Count > 0)
                {
                    List<GameObject> objects = new List<GameObject>();
                    foreach (var col in _stayCollisions)
                    {
                        objects.Add(col.GameObject);
                    }

                    GameObject.OnTriggerStay(objects);
                }
            }
        }
        // 更新所有碰撞体信息
        public void Update(bool debug = false)
        {
            for (int i = 0; i < Collisions.Count; ++i)
            {
                Collisions[i].UpdateCollide();
            }
        }

        public List<Collision> GetEnterCollisions()
        {
            return _enterCollisions;
        }

        public List<Collision> GetExitCollisions()
        {
            return _exitCollisions;
        }

        public List<Collision> GetStayCollisions()
        {
            return _stayCollisions;
        }

        public void Destroy()
        {
            GameObject = null;
            Collisions.Remove(this);
            IsValid = false;
        }

        ~Collision()
        {
            Destroy();
        }
    }
}
