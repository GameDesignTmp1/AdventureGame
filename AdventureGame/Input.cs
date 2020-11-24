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
        public static void ProcessKeyDown(KeyEventArgs e)
        {
            if (!Key.Contains(e.KeyCode))
                Key.Add(e.KeyCode);
        }

        public static void ProcessKeyUp(KeyEventArgs e)
        {
            if (Key.Contains(e.KeyCode))
                Key.Remove(e.KeyCode);
        }

        public static bool IsKeyPress(Keys key)
        {
            return Key.Contains(key);
        }
    }
}
