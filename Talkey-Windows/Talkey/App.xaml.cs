using System.Diagnostics;
using System.Threading;
using System.Windows;

using Handlers;
using Util;

using NotifyIcon = System.Windows.Forms.NotifyIcon;
using ContextMenu = System.Windows.Forms.ContextMenu;
using MenuItem = System.Windows.Forms.MenuItem;

namespace Talkey {
    public partial class App : Application {
        readonly string CHROME_EXTENSION_URL = "https://www.google.com/";
        NotifyIcon trayIcon;
        TrayWindow trayWindow;

        public App() {
            CheckSingleInstance();
            Log.Init();
            InitTrayIcon();
            IPCHandler.Init();
            HookHandler.Init();
            SoundHandler.Init();
            LoadPreferences();
        }

        void OnExit(object sender, ExitEventArgs e) {
            HookHandler.Shutdown();
            IPCHandler.Shutdown();
            SoundHandler.Shutdown();
            trayIcon.Dispose();
        }

        void CheckSingleInstance() {
            Mutex mutex = new Mutex(true, "TalkeyWindowsSingleInstanceMutex");
            if (!mutex.WaitOne(0, true)) {
                MessageBox.Show("Talkey is already running", "Talkey", MessageBoxButton.OK, MessageBoxImage.Information);
                Current.Shutdown();
            }
        }

        void LoadPreferences() {
            KeyHandler.CurrentKeyCombo.Add(VK.LeftControl);
        }

        void InitTrayIcon() {
            MenuItem exitItem = new MenuItem("Exit", (sender, e) => Current.Shutdown());
            MenuItem installChromeItem = new MenuItem("Install Chrome extension", (sender, e) =>
                Process.Start(CHROME_EXTENSION_URL));
            ContextMenu menu = new ContextMenu(new[] { installChromeItem, exitItem });

            trayIcon = new NotifyIcon {
                Icon = Talkey.Properties.Resources.TrayIcon,
                Text = "Talkey (Google Meet Push-to-talk)",
                ContextMenu = menu,
                Visible = true,
            };

            trayIcon.Click += (sender, e) => {
                if (trayWindow == null) {
                    trayWindow = new TrayWindow();
                    trayWindow.Show();
                } else {
                    trayWindow.Activate();
                }
            };
        }
    }
}
