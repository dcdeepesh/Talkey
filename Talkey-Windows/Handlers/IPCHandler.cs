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

        private static EventHandler<KbdLLHookStruct> onKeyPressed, onKeyReleased;

        public static void Init() {
            wsServer = new WebSocketServer(PORT);
            wsServer.AddWebSocketService<WebSocketServerBehaviour>("/",
                behaviour => wsBehaviour = behaviour);
            wsServer.Start();

            onKeyPressed = (sender, kbdInfo) => {
                if (kbdInfo.vkCode == KeyHandler.CurrentPTTKey)
                    wsBehaviour?.SendMute();
            };
            KeyHandler.OnKeyPressed += onKeyPressed;

            onKeyReleased = (sender, kbdInfo) => {
                if (kbdInfo.vkCode == KeyHandler.CurrentPTTKey)
                    wsBehaviour?.SendUnmute();
            };
            KeyHandler.OnKeyReleased += onKeyReleased;
        }

        public static void Shutdown() {
            KeyHandler.OnKeyPressed -= onKeyPressed;
            KeyHandler.OnKeyReleased -= onKeyReleased;

            if (wsServer != null && wsServer.IsListening)
                wsServer.Stop();
            wsServer = null;
        }
    }
}
