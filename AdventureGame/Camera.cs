using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame
{
    public static class Camera
    {
        private static GameObject AttachGameObject = null;

        public static Vec2 GetCenter()
        {
            if (AttachGameObject == null)
                return new Vec2(-100, -100);
            return AttachGameObject.Transform.Location;
        }

        public static void Attach(GameObject obj)
        {
            AttachGameObject = obj;
        }
    }
}
