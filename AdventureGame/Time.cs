using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame
{
    static class Time
    {
        public static DateTime LastTime;
        public static double DeltaTime;
        public static double GetDeltaTime()
        {
            return DeltaTime;
        }

        public static void Update()
        {
            var now = DateTime.Now;
            var delta = now - LastTime;
            DeltaTime = delta.TotalMilliseconds * 0.001;
            LastTime = now;
        }

        public static void Init()
        {
            LastTime = DateTime.Now;
        }
    }
}
