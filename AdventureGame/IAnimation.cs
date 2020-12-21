using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame
{
    // 动画的要根据 Time 类提供的时间进行恒定速率的图片播放
    interface IAnimation
    {
        void LoadAnimation(string texFolder);
        void Draw(Graphics gc, Vec2 offset);
        void Pause();
        void Reset();
        void Destroy();
    }
}
