using System.Windows;

using Handlers;

namespace Talkey {
    public partial class TrayWindow : Window {
        public TrayWindow() {
            InitializeComponent();

            foreach (VK key in KeyHandler.CurrentKeyCombo) {
                Key keyView = new Key();
                keyView.KeyName.Text = key.ToString();
                Keys.Children.Add(keyView);
            };
        }
    }
}
