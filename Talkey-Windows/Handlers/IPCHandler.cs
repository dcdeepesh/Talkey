using System;

using WebSocketSharp;
using WebSocketSharp.Server;

namespace Handlers {
    class WebSocketServerBehaviour : WebSocketBehavior {
        protected override void OnOpen() => IPCHandler.Connected();
        protected override void OnClose(CloseEventArgs e) => IPCHandler.Disconnected();

        public void SendMute() {
            if (State == WebSocketState.Open)
                Send("MUTE");
        }

        public void SendUnmute() {
            if (State == WebSocketState.Open)
                Send("UNMUTE");
        }
    }

    public class IPCHandler {
        const int PORT = 23267;

        static WebSocketServer wsServer;
        static WebSocketServerBehaviour wsBehaviour;

        static EventHandler onMute, onUnmute;

        public static void Init() {
            wsServer = new WebSocketServer(PORT);
            wsServer.AddWebSocketService<WebSocketServerBehaviour>("/",
                behaviour => wsBehaviour = behaviour);
            wsServer.Start();

            onMute = (sender, kbdInfo) => wsBehaviour?.SendMute();
            onUnmute = (sender, kbdInfo) => wsBehaviour?.SendUnmute();
            KeyHandler.OnMute += onMute;
            KeyHandler.OnUnmute += onUnmute;
        }

        public static void Shutdown() {
            KeyHandler.OnMute -= onMute;
            KeyHandler.OnUnmute -= onUnmute;

            if (wsServer != null && wsServer.IsListening)
                wsServer.Stop();
            wsServer = null;
        }

        public static event EventHandler OnConnect;
        public static event EventHandler OnDisconnect;

        internal static void Connected() => OnConnect?.Invoke(null, null);
        internal static void Disconnected() => OnDisconnect?.Invoke(null, null);
    }
}
