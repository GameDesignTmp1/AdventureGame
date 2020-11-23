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
        void Print(int x, int y);
    }
}
