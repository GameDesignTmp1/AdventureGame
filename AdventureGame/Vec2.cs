using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame
{
    public class Vec2
    {
        public double X, Y;

        public Vec2()
        {
            X = Y = 0;
        }
        public Vec2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double Square()
        {
            return X * X + Y * Y;
        }

        public double Length()
        {
            return Math.Sqrt(Square());
        }

        public static Vec2 operator /(Vec2 v, double d)
        {
            var dd = 1 / d;
            return new Vec2(v.X * d, v.Y * d);
        }

        public Vec2 Normalize()
        {
            return this / Length();
        }
    }
}
