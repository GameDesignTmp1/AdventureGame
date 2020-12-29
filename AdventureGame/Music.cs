using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxWMPLib;

namespace AdventureGame
{
    public static class Music
    {
        static AxWindowsMediaPlayer axWindowsMediaPlayer = new AxWindowsMediaPlayer();
        static List<string> music = new List<string>();//存放所有音乐文件路径
        public static void Init(Form form)
        {
            ((System.ComponentModel.ISupportInitialize)(axWindowsMediaPlayer)).BeginInit();
            form.Controls.Add(axWindowsMediaPlayer);
            ((System.ComponentModel.ISupportInitialize)(axWindowsMediaPlayer)).EndInit();
            axWindowsMediaPlayer.Visible = false;
            LoadMusic();
            Play("游戏胜利.mp3");
        }
        public static float GetTotalTime()
        {
            return 0;

        }

        public static void LoadMusic()
        {
            DirectoryInfo dir = new DirectoryInfo(".\\music");
            FileInfo[] files = dir.GetFiles();
            DirectoryInfo[] mDirs = dir.GetDirectories();
            foreach (FileInfo file in files)
            {
                music.Add(file.FullName);//将文件路径加到列表中
            }

        }

        public static void Pause()
        {
            axWindowsMediaPlayer.Ctlcontrols.pause();

        }

        public static void Play(string filename)
        {
            foreach (var m in music)
            {
                if (m.Split('\\').Last() == filename)
                {
                    axWindowsMediaPlayer.URL = m;
                    axWindowsMediaPlayer.Ctlcontrols.play();
                }
            }
        }
    }
}
