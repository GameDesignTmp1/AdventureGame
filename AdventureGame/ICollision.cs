using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame
{
    // Collision 要判断是否发生碰撞，并且给出某个方向可移动的距离
    interface ICollision
    {
        // 给出某个方向上 x、y、z 可以移动的最大距离（注意是三个轴在 moveDir 内各自的最大距离）
        Vec2 GetMoveDis(Vec2 moveDir);
        List<Collision> GetEnterCollisions();
        List<Collision> GetExitCollisions();
        List<Collision> GetStayCollisions();
    }
}
