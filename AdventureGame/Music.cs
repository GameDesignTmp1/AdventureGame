using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxWMPLib;

namespace AdventureGame
{
    public class Music:IMusic
    {
        AxWindowsMediaPlayer axWindowsMediaPlayer = new AxWindowsMediaPlayer();

        public Music(Form form)
        {
            form.Controls.Add(axWindowsMediaPlayer);
            axWindowsMediaPlayer.Visible = false;
        }
        public float GetTotalTime()
        {
            return 0;

        }

        public void LoadMusic(string filename)
        {

            axWindowsMediaPlayer.URL = filename;

        }

        public void Pause()
        {
            axWindowsMediaPlayer.Ctlcontrols.pause();

        }

        public void Play()
        {
            axWindowsMediaPlayer.Ctlcontrols.play();
        }
    }
}
