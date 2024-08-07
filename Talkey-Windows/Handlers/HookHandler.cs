﻿using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

using Util;

namespace Handlers {
    class WH {
        public const int KEYBOARD    = 2;
        public const int MOUSE       = 7;
        public const int KEYBOARD_LL = 13;
        public const int MOUSE_LL    = 14;
    }

    public class HookHandler {
        const string DLL_FILE = "Hook.dll";

        const string REGISTER_CALLBACK_FUNCTION_NAME = "RegisterCallback";
        const string LLKEYBOARDPROC_FUNCTION_NAME = "LowLevelKeyboardProc";

        /*
        const string REGISTER_CALLBACK_FUNCTION_NAME = "_RegisterCallback@4";
        const string LLKEYBOARDPROC_FUNCTION_NAME = "_LowLevelKeyboardProc@12";
        */

        static IntPtr hDll, hHook;
        static IntPtr lpfnRegisterCallback, lpfnLLKeyboardProc;

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

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadLibrary(string dllToLoad);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeLibrary(IntPtr hModule);

        static void LoadProcedures() {
            hDll = LoadLibrary(DLL_FILE);
            if (hDll == IntPtr.Zero) {
                Log.E($"DLL loading error: {new Win32Exception(Marshal.GetLastWin32Error()).Message}");
                return;
            }

            lpfnRegisterCallback = GetProcAddress(hDll, REGISTER_CALLBACK_FUNCTION_NAME);
            if (lpfnRegisterCallback == IntPtr.Zero) {
                Log.E($"RegisterCallback not found: {new Win32Exception(Marshal.GetLastWin32Error()).Message}");
                return;
            }

            lpfnLLKeyboardProc = GetProcAddress(hDll, LLKEYBOARDPROC_FUNCTION_NAME);
            if (lpfnLLKeyboardProc == IntPtr.Zero) {
                Log.E($"LLKeyboardProc not found: {new Win32Exception(Marshal.GetLastWin32Error()).Message}");
                return;
            }

            Log.I("Loaded all procedures");
        }

        #endregion

        #region Register callback

        private delegate void KeyEventCallback(IntPtr wParam, IntPtr lParam);
        private static KeyEventCallback callback;

        private delegate void DRegisterCallback(KeyEventCallback callback);

        static void RegisterCallback() {
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

        static void Hook() {
            LLKeyboardProc llKdbProc = (LLKeyboardProc)
                Marshal.GetDelegateForFunctionPointer(lpfnLLKeyboardProc, typeof(LLKeyboardProc));
            hHook = SetWindowsHookEx(WH.KEYBOARD_LL, llKdbProc, hDll, 0);
            if (hHook == IntPtr.Zero) {
                Log.E("Hook unsuccessful");
                return;
            }

            Log.I("Hooked");
        }

        #endregion

        #region Unhook

        static void Unhook() {
            bool unhooked = UnhookWindowsHookEx(hHook);
            if (!unhooked)
                Log.E("Unhook unsuccessful");

            Log.I("Unhooked");
        }

        #endregion

        #region Key event callback

        static void OnKeyEvent(IntPtr wParam, IntPtr lParam) {
            KeyHandler.OnKeyEvent(wParam, lParam);
        }

        #endregion
    }
}
