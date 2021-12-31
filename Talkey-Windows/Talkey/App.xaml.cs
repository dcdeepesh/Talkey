using Handlers;

using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;

using Util;

using ContextMenu = System.Windows.Forms.ContextMenu;
using MenuItem = System.Windows.Forms.MenuItem;
using NotifyIcon = System.Windows.Forms.NotifyIcon;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using MouseButtons = System.Windows.Forms.MouseButtons;

namespace Talkey {
    public partial class App : Application {
        const string CHROME_EXTENSION_URL = "https://chrome.google.com/webstore/detail/ikpllienmchnfkfbfindmciobnhdgjlh";
        NotifyIcon trayIcon;
        readonly TrayWindow trayWindow;
        readonly bool toRun;

        public App() {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            toRun = CheckSingleInstance();
            if (toRun) {
                Log.Init();
                InitGlobalExceptionHandler();
                InitTrayIcon();
                IPCHandler.Init();
                HookHandler.Init();
                SoundHandler.Init();
                Preferences.Load();
                trayWindow = new TrayWindow();
            }
        }

        void OnExit(object sender, ExitEventArgs e) {
            if (toRun) {
                HookHandler.Shutdown();
                IPCHandler.Shutdown();
                SoundHandler.Shutdown();
                trayIcon?.Dispose();
            }
        }

        bool CheckSingleInstance() {
            Mutex mutex = new Mutex(true, "TalkeyWindowsSingleInstanceMutex");
            if (!mutex.WaitOne(0, true)) {
                MessageBox.Show("Talkey is already running", "Talkey", MessageBoxButton.OK, MessageBoxImage.Information);
                Current.Shutdown();
                return false;
            }
            return true;
        }

        void InitGlobalExceptionHandler() {
            AppDomain.CurrentDomain.UnhandledException +=
                (sender, e) => Log.E(e.ExceptionObject.ToString());
        }

        void InitTrayIcon() {
            MenuItem exitItem = new MenuItem("Exit", (sender, e) => Current.Shutdown());
            MenuItem installChromeItem = new MenuItem("Install Chrome extension", (sender, e) =>
                Process.Start(CHROME_EXTENSION_URL));
            ContextMenu menu = new ContextMenu(new[] { installChromeItem, exitItem });

            trayIcon = new NotifyIcon {
                Icon = Talkey.Properties.Resources.TrayIconOff,
                Text = "Talkey (Google Meet Push-to-talk)",
                ContextMenu = menu,
                Visible = true,
            };

            IPCHandler.OnConnect += (sender, args) =>
                trayIcon.Icon = Talkey.Properties.Resources.TrayIcon;
            IPCHandler.OnDisconnect += (sender, args) =>
                trayIcon.Icon = Talkey.Properties.Resources.TrayIconOff;

            trayIcon.Click += (sender, e) => {
                if (e is MouseEventArgs mouseEvent && mouseEvent.Button != MouseButtons.Left)
                    return;
                trayWindow.SetWindowPosition();
                trayWindow.Show();
                trayWindow.Activate();
            };
        }
    }
}
