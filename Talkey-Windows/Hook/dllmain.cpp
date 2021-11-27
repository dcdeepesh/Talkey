#include "pch.h"

BOOL APIENTRY DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved) {
    return TRUE;
}

using KeyEventCallback = void(__stdcall*)(WPARAM, LPARAM);
KeyEventCallback callback;

extern "C" {
    __declspec(dllexport) void __stdcall RegisterCallback(KeyEventCallback callback) {
        ::callback = callback;
    }

    __declspec(dllexport) LRESULT CALLBACK LowLevelKeyboardProc(int nCode, WPARAM wParam, LPARAM lParam) {
        if (nCode >= 0)
            callback(wParam, lParam);

        return CallNextHookEx(NULL, nCode, wParam, lParam);
    }
}