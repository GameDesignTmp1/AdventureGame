using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame
{
    // 加载并保存图片，提供输出图片功能
    interface ITexture
    {
        void LoadTexture(string filename);
        // 作为静态函数实现，将所有贴图由深到浅打印（参考我展示的 2D 游戏）
        void Print();
    }
}
