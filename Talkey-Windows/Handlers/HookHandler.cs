using System;
using System.Runtime.InteropServices;

using Util;

namespace Handlers {
    class WH {
        public static readonly int KEYBOARD    = 2;
        public static readonly int MOUSE       = 7;
        public static readonly int KEYBOARD_LL = 13;
        public static readonly int MOUSE_LL    = 14;
    }

    public class HookHandler {
        //private static readonly string DLL_FILE = "C:\\Users\\dcdee\\CC\\Programs\\Talkey\\Debug\\Hook.dll";
        private static readonly string DLL_FILE = "Hook.dll";

        private static IntPtr hDll, hHook;
        private static IntPtr lpfnRegisterCallback, lpfnLLKeyboardProc;

        public static void Init() {
            Log.I("INIT");

            LoadProcedures();
            RegisterCallback();
            Hook();
        }

        public static void Shutdown() {
            Unhook();
            FreeLibrary(hDll);

            Log.I("SHUTDOWN");
        }

        #region Load library and procedures

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string dllToLoad);
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);
        [DllImport("kernel32.dll")]
        private static extern bool FreeLibrary(IntPtr hModule);

        private static void LoadProcedures() {
            hDll = LoadLibrary(DLL_FILE);
            if (hDll == null) {
                Log.E("DLL not found");
                return;
            }

            lpfnRegisterCallback = GetProcAddress(hDll, "_RegisterCallback@4");
            if (lpfnRegisterCallback == null) {
                Log.E("RegisterCallback not found");
                return;
            }

            lpfnLLKeyboardProc = GetProcAddress(hDll, "_LowLevelKeyboardProc@12");
            if (lpfnLLKeyboardProc == null) {
                Log.E("LLKeyboardProc not found");
                return;
            }

            Log.I("Loaded all procedures");
        }

        #endregion

        #region Register callback

        private delegate void KeyEventCallback(IntPtr wParam, IntPtr lParam);
        private static KeyEventCallback callback;

        private delegate void DRegisterCallback(KeyEventCallback callback);

        private static void RegisterCallback() {
            DRegisterCallback registerCallback = (DRegisterCallback)
                Marshal.GetDelegateForFunctionPointer(lpfnRegisterCallback, typeof(DRegisterCallback));
            callback = new KeyEventCallback(OnKeyEvent);
            registerCallback(callback);

            Log.I("Callback registered");
        }

        #endregion

        #region Hook

        private delegate IntPtr LLKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LLKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        private static void Hook() {
            LLKeyboardProc llKdbProc = (LLKeyboardProc)
                Marshal.GetDelegateForFunctionPointer(lpfnLLKeyboardProc, typeof(LLKeyboardProc));
            hHook = SetWindowsHookEx(WH.KEYBOARD_LL, llKdbProc, hDll, 0);
            if (hHook == null) {
                Log.E("Hook unsuccessful");
                return;
            }

            Log.I("Hooked");
        }

        #endregion

        #region Unhook

        private static void Unhook() {
            bool unhooked = UnhookWindowsHookEx(hHook);
            if (!unhooked)
                Log.E("Unhook unsuccessful");

            Log.I("Unhooked");
        }

        #endregion

        #region Key event callback

        private static void OnKeyEvent(IntPtr wParam, IntPtr lParam) {
            KeyHandler.OnKeyEvent(wParam, lParam);
        }

        #endregion
    }
}
