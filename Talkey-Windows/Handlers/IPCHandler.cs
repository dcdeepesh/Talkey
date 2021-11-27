using WebSocketSharp.Server;

namespace Handlers {
    class WebSocketServerBehaviour : WebSocketBehavior {
        protected override void OnOpen() => Send("MUTE");

        public void SendMute() => Send("MUTE");
        public void SendUnmute() => Send("UNMUTE");
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
