using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame
{
    interface IMusic
    {
        void LoadMusic(string filename);
        void Play();
        void Pause();
        float GetTotalTime();
    }
}
