using System;
using System.Collections.Generic;
using System.Globalization;
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

        public Vec2(Vec2 v)
        {
            X = v.X;
            Y = v.Y;
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
            return new Vec2(v.X * dd, v.Y * dd);
        }

        public static Vec2 operator *(Vec2 v, double d)
        {
            return new Vec2(v.X * d, v.Y * d);
        }

        public static Vec2 operator -(Vec2 v1, Vec2 v2)
        {
            return new Vec2(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vec2 operator -(Vec2 v)
        {
            return new Vec2(-v.X, -v.Y);
        }
        public override string ToString()
        {
            return "x:" + X.ToString(CultureInfo.InvariantCulture) + 
                   ", y:" + Y.ToString(CultureInfo.InvariantCulture);
        }

        public static Vec2 operator +(Vec2 v, double d)
        {
            return new Vec2(v.X + d, v.Y + d);
        }

        public static Vec2 operator +(Vec2 v1, Vec2 v2)
        {
            return new Vec2(v1.X + v2.X, v1.Y + v2.Y);
        }

        public Vec2 Clone()
        {
            return new Vec2(X, Y);
        }

        public void Reset()
        {
            X = Y = 0;
        }

        public Vec2 Normalize()
        {
            var len = Length();
            if (Length() < 0.001)
                return this * 10000;
            return this / len;
        }
    }
}
