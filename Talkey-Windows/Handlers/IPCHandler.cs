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

        public static void Init() {
            wsServer = new WebSocketServer(PORT);
            wsServer.AddWebSocketService<WebSocketServerBehaviour>("/",
                behaviour => wsBehaviour = behaviour);
            wsServer.Start();
        }

        public static void Shutdown() {
            if (wsServer != null && wsServer.IsListening)
                wsServer.Stop();
            wsServer = null;
        }

        public static void SendMute() => wsBehaviour?.SendMute();
        public static void SendUnmute() => wsBehaviour?.SendUnmute();
    }
}
