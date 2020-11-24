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
        public static DateTime StartTime = DateTime.Now;
        public static double DeltaTime;
        public static double TotalTime;
        private static long _cnt = 0;
        // 修正 deltaTime 的比率
        private static double _fixRate = 0.1;
        public static double GetDeltaTime()
        {
            return DeltaTime;
        }

        public static void Update()
        {
            _cnt++;
            var now = DateTime.Now;
            var delta = now - LastTime;
            var tmp = delta.TotalMilliseconds * 0.001;
            DeltaTime = DeltaTime * (1 - _fixRate) + _fixRate * tmp;
            LastTime = now;
            TotalTime = (now - StartTime).Milliseconds * 0.001;
        }

        public static void Init()
        {
            LastTime = DateTime.Now;
        }
    }
}
