using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame
{
    class Vec3
    {
        public double X, Y, Z;

        public Vec3()
        {
            X = Y = Z = 0;
        }
        public Vec3(double v)
        {
            X = Y = Z = v;
        }

        public Vec3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vec3(Vec3 v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        public double Square()
        {
            return X * X + Y * Y + Z * Z;
        }

        public double Length()
        {
            return Math.Sqrt(Square());
        }

        public static Vec3 operator *(Vec3 v1, double v)
        {
            return new Vec3(v1.X*v,v1.Y*v,v1.Z*v);
        }
        public static Vec3 operator/(Vec3 v1, double v)
        {
            return v1 * (1 / v);
        }
        public Vec3 Normalize()
        {
            return this / Length();
        }
        
    }
}
