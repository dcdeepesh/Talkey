using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

using Microsoft.Win32;
using Handlers;
using System.Reflection;

namespace Talkey {
    public partial class TrayWindow : Window {
        public TrayWindow() {
            InitializeComponent();
            ShowCurrentKeyCombo();
            cbActivate.IsChecked = Preferences.cbActivate;
            cbDeactivate.IsChecked = Preferences.cbDeactivate;
            cbStartup.IsChecked = Preferences.cbStartup;
        }

        protected override void OnClosing(CancelEventArgs e) {
            Hide();
            e.Cancel = true;
        }

        void ShowCurrentKeyCombo() {
            foreach (VK key in KeyHandler.CurrentKeyCombo) {
                Key keyView = new Key();
                keyView.KeyName.Text = key.ToString();
                Keys.Children.Add(keyView);
            };
        }

        void OnClickChange(object sender, RoutedEventArgs e) {
            ChangeButton.Visibility = Visibility.Collapsed;
            ResetButton.Visibility = Visibility.Visible;
            DoneButton.Visibility = Visibility.Visible;
            KeyAdvice.Visibility = Visibility.Visible;
            BeginChangingKeyCombo();
        }

        void OnClickReset(object sender, RoutedEventArgs e) {
            ResetKeyCombo();
        }

        void OnClickDone(object sender, RoutedEventArgs e) {
            EndChangingKeyCombo();
            ChangeButton.Visibility = Visibility.Visible;
            ResetButton.Visibility = Visibility.Collapsed;
            DoneButton.Visibility = Visibility.Collapsed;
            KeyAdvice.Visibility = Visibility.Collapsed;
        }

        #region Key combo changing logic

        readonly HashSet<VK> newCombo = new HashSet<VK>();

        void BeginChangingKeyCombo() {
            ResetKeyCombo();
            KeyHandler.OnKeyPressed += OnKeyPressed;
        }

        void ResetKeyCombo() {
            newCombo.Clear();
            Keys.Children.Clear();
        }

        void EndChangingKeyCombo() {
            KeyHandler.OnKeyPressed -= OnKeyPressed;
            if (newCombo.Count != 0) {
                KeyHandler.ChangeCurrentKeyCombo(newCombo);
                Preferences.StoreKeyCombo(newCombo);
                ResetKeyCombo();
            }
            ShowCurrentKeyCombo();
        }

        void OnKeyPressed(object sender, KbdLLHookStruct kbdInfo) {
            if (newCombo.Add(kbdInfo.vkCode)) {
                Key key = new Key();
                key.KeyName.Text = kbdInfo.vkCode.ToString();
                Keys.Children.Add(key);
            }
        }

        #endregion

        #region Checkbox handlers

        private void OnCBActivateChange(object sender, RoutedEventArgs e) =>
            Preferences.StoreCBValue("cbActivate", cbActivate.IsChecked.GetValueOrDefault());

        private void OnCBDectivateChange(object sender, RoutedEventArgs e) =>
            Preferences.StoreCBValue("cbDeactivate", cbDeactivate.IsChecked.GetValueOrDefault());

        private void OnCBStartupChange(object sender, RoutedEventArgs e) {
            string REGISTRY_SUBKEY_STARTUP = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";

            RegistryKey key = Registry.CurrentUser.CreateSubKey(REGISTRY_SUBKEY_STARTUP);
            bool newState = cbStartup.IsChecked.GetValueOrDefault();
            if (newState)
                key.SetValue("Talkey", Assembly.GetEntryAssembly().Location, RegistryValueKind.String);
            else
                try { key.DeleteValue("Talkey"); } catch (ArgumentException) { }
            key.Dispose();

            Preferences.StoreCBValue("cbStartup", newState);
        }

        #endregion
    }
}
