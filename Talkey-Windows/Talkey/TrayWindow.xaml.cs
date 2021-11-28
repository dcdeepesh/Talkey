using System.Windows;
using System.Collections.Generic;

using Handlers;

namespace Talkey {
    public partial class TrayWindow : Window {
        public TrayWindow() {
            InitializeComponent();
            ShowCurrentKeyCombo();
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

        HashSet<VK> newCombo = new HashSet<VK>();

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
    }
}
