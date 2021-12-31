using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Util;

namespace Handlers {
#pragma warning disable CS0649
    public struct KbdLLHookStruct {
        public VK vkCode;
        public uint scanCode;
        public uint flags;
        public uint time;
        public IntPtr dwExtraInfo;
    }
#pragma warning restore CS0649

    public class KeyHandler {
        private static readonly HashSet<VK> PressedKeys = new HashSet<VK>();

        public static event EventHandler<KbdLLHookStruct> OnKeyPressed;
        public static event EventHandler<KbdLLHookStruct> OnKeyReleased;
        public static event EventHandler OnMute;
        public static event EventHandler OnUnmute;

        #region Logic for OnMute and OnUnmute

        static bool mute = true;

        static KeyHandler() {
            OnKeyPressed += (sender, args) => {
                PressedKeys.Add(args.vkCode);
                if (mute && CheckUnmuteCondition()) {
                    mute = false;
                    OnUnmute?.Invoke(new object(), new EventArgs());
                }
            };

            OnKeyReleased += (sender, args) => {
                PressedKeys.Remove(args.vkCode);
                if (!mute && !CheckUnmuteCondition()) {
                    mute = true;
                    OnMute?.Invoke(new object(), new EventArgs());
                }
            };
        }

        #endregion

        public static void ChangeCurrentKeyCombo(HashSet<VK> newKeyCombo) {
            Preferences.CurrentKeyCombo.Clear();
            Preferences.CurrentKeyCombo.UnionWith(newKeyCombo);
        }

        public static void OnKeyEvent(IntPtr wParam, IntPtr lParam) {
            KbdLLHookStruct kbdInfo = (KbdLLHookStruct)
                Marshal.PtrToStructure(lParam, typeof(KbdLLHookStruct));

            if (wParam == new IntPtr(WM.KEYDOWN))
                OnKeyPressed?.Invoke(null, kbdInfo);
            else if (wParam == new IntPtr(WM.KEYUP))
                OnKeyReleased?.Invoke(null, kbdInfo);
            else if (wParam == new IntPtr(WM.SYSKEYDOWN))
                OnKeyPressed?.Invoke(null, kbdInfo);
            else if (wParam == new IntPtr(WM.SYSKEYUP))
                OnKeyReleased?.Invoke(null, kbdInfo);
            else
                Log.E("Key handler not found");
        }

        static bool CheckUnmuteCondition() {
            foreach (VK key in Preferences.CurrentKeyCombo)
                if (!PressedKeys.Contains(key))
                    return false;

            return true;
        }
    }

    class WM {
        public const int KEYDOWN    = 0x0100;
        public const int KEYUP      = 0x0101;
        public const int SYSKEYDOWN = 0x0104;
        public const int SYSKEYUP   = 0x0105;
    }
}
