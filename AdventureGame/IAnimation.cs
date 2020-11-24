using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame
{
    // 动画的要根据 Time 类提供的时间进行恒定速率的图片播放
    interface IAnimation
    {
        void Play();
        void Pause();
        void Reset();
    }
}
