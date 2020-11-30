using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace AdventureGame
{
    public static class Scene
    {
        public static Vec2 Offset;
        public static void LoadScene(string filename)
        {
            byte[] str = new byte[10240];
            int length = File.Open(filename, FileMode.Open).Read(str, 0, 10240);
            string s = Encoding.Default.GetString(str);
            List<SceneForm.Saver> tmpList;
            try
            {
                tmpList = JsonConvert.DeserializeObject<List<SceneForm.Saver>>(s);
            }
            catch (Exception exception)
            {
                MessageBox.Show("场景文件加载错误，必须是合法的 Json 文件且不能有中文");
                return;
            }

            for (int i = GameObject.GameObjects.Count - 1; i >= 0; --i)
            {
                GameObject.GameObjects[i].Destroy();
            }

            foreach (var tp in tmpList)
            {
                var obj = GenObjectFromTag(tp.Tag, tp.X, tp.Y);
                ObjCopy(obj, tp);
                ObjCopy(obj.collision, tp);
            }
        }

        private static GameObject GenObjectFromTag(string tag, double x, double y)
        {
            GameObject obj = new GameObject(x, y);
            switch (tag)
            {
                case "Player":
                    break;
                case "Background":
                    break;
                case "Building":
                    break;
                default:
                    break;
            }

            return obj;
        }

        public static void ObjCopy(Object target, Object src)
        {
            var info1 = target.GetType().GetFields();
            var info2 = src.GetType().GetFields();
            foreach (var targetInfo in info1)
            {
                foreach (var srcInfo in info2)
                {
                    if (srcInfo.Name == targetInfo.Name)
                    {
                        targetInfo.SetValue(target, srcInfo.GetValue(src));
                        break;
                    }
                }
            }
        }
    }
}
