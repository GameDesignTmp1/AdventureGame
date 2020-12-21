using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdventureGame
{
    public static class Camera
    {
        private static GameObject _attachGameObject = null;
        private static Vec2 _preCenter = null;
        private static Form1 _form = null;
        private static double _speed = 0.01;
        private static double _dis = 60;
        private static bool _chase = false;
        public static void Init(Form1 frm)
        {
            _form = frm;
        }
        public static Vec2 GetCenter()
        {
            if (_attachGameObject == null || _form == null)
                return new Vec2(-100, -100);
            var center = _attachGameObject.Transform.Location -
                         new Vec2(_form.Width, _form.Height) / 2;
            var d = center - _preCenter;
            if (!_chase && d.Length() > _dis)
                _chase = true;
            else if (d.Length() < 5)
                _chase = false;

            if (_chase)
            {
                _preCenter += d * _speed;
            }
            return _preCenter;
        }

        public static void Attach(GameObject obj)
        {
            _attachGameObject = obj;
            _preCenter = _attachGameObject.Transform.Location;
        }
    }
}
