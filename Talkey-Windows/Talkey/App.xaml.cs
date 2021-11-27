using System.Windows;

using Handlers;

using NotifyIcon = System.Windows.Forms.NotifyIcon;
using ContextMenu = System.Windows.Forms.ContextMenu;
using MenuItem = System.Windows.Forms.MenuItem;

namespace Talkey {
    public partial class App : Application {
        NotifyIcon trayIcon;

        public App() {
            HookHandler.Init();
            IPCHandler.Init();
            InitTrayIcon();
        }

        private void OnExit(object sender, ExitEventArgs e) {
            HookHandler.Shutdown();
            IPCHandler.Shutdown();
            trayIcon.Dispose();
        }

        private void InitTrayIcon() {
            MenuItem menuItem = new MenuItem("Exit", (sender, e) => Application.Current.Shutdown());
            ContextMenu menu = new ContextMenu(new[] { menuItem });

            trayIcon = new NotifyIcon {
                Icon = Talkey.Properties.Resources.TrayIcon,
                Text = "Talkey (Google Meet Push-to-talk)",
                ContextMenu = menu,
                Visible = true
            };
        }
    }
}
