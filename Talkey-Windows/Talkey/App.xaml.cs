using System.Threading;
using System.Windows;

using Handlers;

using NotifyIcon = System.Windows.Forms.NotifyIcon;
using ContextMenu = System.Windows.Forms.ContextMenu;
using MenuItem = System.Windows.Forms.MenuItem;

namespace Talkey {
    public partial class App : Application {
        NotifyIcon trayIcon;

        public App() {
            CheckSingleInstance();
            IPCHandler.Init();
            InitTrayIcon();
            HookHandler.Init();
        }

        void OnExit(object sender, ExitEventArgs e) {
            HookHandler.Shutdown();
            IPCHandler.Shutdown();
            trayIcon.Dispose();
        }

        void CheckSingleInstance() {
            Mutex mutex = new Mutex(true, "TalkeyWindowsSingleInstanceMutex");
            if (!mutex.WaitOne(0, true)) {
                MessageBox.Show("Talkey is already running", "Talkey", MessageBoxButton.OK, MessageBoxImage.Information);
                Current.Shutdown();
            }
        }

        void InitTrayIcon() {
            MenuItem menuItem = new MenuItem("Exit", (sender, e) => Current.Shutdown());
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
