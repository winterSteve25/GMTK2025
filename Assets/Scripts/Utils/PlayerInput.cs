using UnityEngine;

namespace Utils
{
    public class PlayerInput
    {
        public static bool Disabled = false;

        public static float GetAxisRaw(string axis)
        {
            if (Disabled) return 0;
            return Input.GetAxisRaw(axis);
        }

        public static bool KeyDown(KeyCode keyCode)
        {
            if (Disabled) return false;
            return Input.GetKeyDown(keyCode);
        }

        public static bool MouseDown(int button)
        {
            if (Disabled) return false;
            return Input.GetMouseButtonDown(button);
        }
        
        public static bool Mouse(int button)
        {
            if (Disabled) return false;
            return Input.GetMouseButton(button);
        }
    }
}