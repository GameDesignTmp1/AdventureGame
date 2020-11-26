using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame
{
    // 变换类接口的任务是根据 Input 类提供的碰撞信息进行移动或者旋转
    interface ITransform
    {
        void Translate(Vec2 moveDir);
        void TurnLeft();
        void TurnRight();
        void Destroy();
    }
}
