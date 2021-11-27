using System.Windows;

using Handlers;

namespace Talkey {
    public partial class App : Application {
        public App() {
            HookHandler.Init();
            IPCHandler.Init();
        }

        private void OnExit(object sender, ExitEventArgs e) {
            HookHandler.Shutdown();
            IPCHandler.Shutdown();
        }
    }
}
