using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdventureGame
{
    public static class Input
    {
        public static HashSet<Keys> Key = new HashSet<Keys>();

        // 处理按键按下
        public static void ProcessKeyDown(KeyEventArgs e)
        {
            if (!Key.Contains(e.KeyCode))
                Key.Add(e.KeyCode);
        }
        // 处理按键松开
        public static void ProcessKeyUp(KeyEventArgs e)
        {
            if (Key.Contains(e.KeyCode))
                Key.Remove(e.KeyCode);
        }
        // 获取输入
        public static bool IsKeyPress(Keys key)
        {
            return Key.Contains(key);
        }
    }
}
