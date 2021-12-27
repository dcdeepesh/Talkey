using Handlers;

using Microsoft.Win32;

using System;
using System.Collections.Generic;

namespace Talkey {
    public class Preferences {
        const string REGISTRY_SUBKEY_TALKEY = "Software\\Dec\\Talkey";

        public static bool cbActivate, cbDeactivate, cbStartup;
        public static VK[] keyCombo;

        public static void Load() {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(REGISTRY_SUBKEY_TALKEY);

            cbActivate = (int) key.GetValue("cbActivate", 1) == 1;
            cbDeactivate = (int) key.GetValue("cbDeactivate", 1) == 1;
            cbStartup = (int) key.GetValue("cbStartup", 1) == 1;

            KeyHandler.CurrentKeyCombo.Clear();
            string[] keyNames = key.CreateSubKey("keys").GetValueNames();
            if (keyNames.Length > 0) {
                foreach (string value in keyNames)
                    KeyHandler.CurrentKeyCombo.Add((VK) int.Parse(value));
            } else {
                KeyHandler.CurrentKeyCombo.Add(VK.LeftControl);
            }

            key.Dispose();
        }

        public static void StoreCBValue(string cbName, bool isChecked) {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(REGISTRY_SUBKEY_TALKEY);
            key.SetValue(cbName, isChecked ? 1 : 0, RegistryValueKind.DWord);
            key.Dispose();
        }

        public static void StoreKeyCombo(HashSet<VK> newCombo) {
            try {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(REGISTRY_SUBKEY_TALKEY);
                key.DeleteSubKey("keys");
                RegistryKey keysKey = key.CreateSubKey("keys");
                foreach (VK vKey in newCombo)
                    keysKey.SetValue(((int) vKey).ToString(), 0, RegistryValueKind.DWord);
                key.Dispose();
            } catch (ArgumentException) {
                // Thrown when subkey doesn't exist. No need to handle
            }
        }
    }
}
