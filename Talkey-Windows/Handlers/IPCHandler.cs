using System;

using WebSocketSharp;
using WebSocketSharp.Server;

namespace Handlers {
    class WebSocketServerBehaviour : WebSocketBehavior {
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
        private static readonly int PORT = 23267;

        private static WebSocketServer wsServer;
        private static WebSocketServerBehaviour wsBehaviour;

        private static EventHandler onMute, onUnmute;

        public static void Init() {
            wsServer = new WebSocketServer(PORT);
            wsServer.AddWebSocketService<WebSocketServerBehaviour>("/",
                behaviour => wsBehaviour = behaviour);
            wsServer.Start();

            onMute = (sender, kbdInfo) => wsBehaviour?.SendMute();
            KeyHandler.OnMute += onMute;
            onUnmute = (sender, kbdInfo) => wsBehaviour?.SendUnmute();
            KeyHandler.OnUnmute += onUnmute;
        }

        public static void Shutdown() {
            KeyHandler.OnMute -= onMute;
            KeyHandler.OnUnmute -= onUnmute;

            if (wsServer != null && wsServer.IsListening)
                wsServer.Stop();
            wsServer = null;
        }
    }
}
