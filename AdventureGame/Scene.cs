﻿using System;
using System.Collections.Generic;
using System.Drawing;
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
        public static Control DebugControl = null;
        public static List<string> Tags = new List<string>();
        public static Player Player = null;

        private static void InitTags()
        {
            Tags.Add("Building");
            Tags.Add("Player");
            Tags.Add("Enemy");
            Tags.Add("Weapon");
            Tags.Add("Coin");
            Tags.Add("Background");
            Tags.Add("Foreground");
        }
        public static void LoadScene(string filename)
        {
            InitTags();
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
                MessageBox.Show("场景文件加载错误，必须是合法的 Json 文件且不能有中文:"
                                + exception.ToString());
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
                ObjCopy(obj.Collision, tp);
                obj.Texture.LoadTexture(tp.Filename);
                ObjCopy(obj.Texture, tp);
                obj.Texture.Resize(obj.Texture.Width, obj.Texture.Height);
                obj.InitAnimation();
            }
        }

        public static void Update(Graphics gc)
        {
            Time.Update();
            GameObject.Update(gc, true);
        }
        private static GameObject GenObjectFromTag(string tag, double x, double y)
        {
            GameObject obj = null;
            if (!Tags.Contains(tag))
                tag = "Building";
            switch (tag)
            {
                case "Player":
                    obj = new Player(x, y);
                    obj.Depth = 0;
                    var p = (obj as Player);
                    Camera.Attach(obj);
                    Player = p;
                    p.Animations.Add("Walk",
                        new TmpAnimation(obj, ".\\image\\PlayerIdle"));
                    break;
                case "Background":
                    obj = new GameObject(x, y, -1);
                    break;
                case "Building":
                    obj = new GameObject(x, y);
                    obj.Depth = 0;
                    break;
                case "ForeGround":
                    obj = new GameObject(x, y);
                    obj.Depth = 1;
                    break;
                case "Weapon":
                    obj = new GameObject(x, y);
                    obj.Depth = 0;
                    break;
                case "Coin":
                    obj = new Item(x, y, 0, "Coin");
                    break;
                case "Enemy":
                    obj = new Enemy(x, y);
                    ((Enemy) obj).Animations.Add("Idle", 
                        new TmpAnimation(obj, ".\\image\\EnemyIdle"));
                    break;
                default:
                    obj = new GameObject(x, y);
                    break;
            }

            return obj;
        }

        public static void ObjCopy(Object target, Object src)
        {
            if (target is null || src is null)
                return;
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
