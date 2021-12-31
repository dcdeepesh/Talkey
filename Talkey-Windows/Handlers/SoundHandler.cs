using System;
using System.Media;

using Util;

namespace Handlers {
    public class SoundHandler {
        static SoundPlayer activatePlayer, deactivatePlayer;
        static bool playSound;

        public static void Init() {
            activatePlayer = new SoundPlayer(Properties.Resources.PTTActivate);
            deactivatePlayer = new SoundPlayer(Properties.Resources.PTTDeactivate);
            KeyHandler.OnMute += OnMute;
            KeyHandler.OnUnmute += OnUnmute;
            IPCHandler.OnConnect += OnConnect;
            IPCHandler.OnDisconnect += OnDisconnect;
        }

        public static void Shutdown() {
            KeyHandler.OnMute -= OnMute;
            KeyHandler.OnUnmute -= OnUnmute;
            IPCHandler.OnConnect -= OnConnect;
            IPCHandler.OnDisconnect -= OnDisconnect;
        }

        public static void OnConnect(object sender, EventArgs e) => playSound = true;
        public static void OnDisconnect(object sender, EventArgs e) => playSound = false;

        public static void OnMute(object sender, EventArgs args) {
            if (playSound && Preferences.cbDeactivate)
                deactivatePlayer.Play();
        }

        public static void OnUnmute(object sender, EventArgs args) {
            if (playSound && Preferences.cbActivate)
                activatePlayer.Play();
        }
    }
}