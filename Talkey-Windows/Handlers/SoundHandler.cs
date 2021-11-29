using System;
using System.Media;

namespace Handlers {
    public class SoundHandler {
        static SoundPlayer activatePlayer, deactivatePlayer;

        public static void Init() {
            activatePlayer = new SoundPlayer(Properties.Resources.PTTActivate);
            deactivatePlayer = new SoundPlayer(Properties.Resources.PTTDeactivate);
            KeyHandler.OnMute += OnMute;
            KeyHandler.OnUnmute += OnUnmute;
        }

        public static void Shutdown() {
            KeyHandler.OnMute -= OnMute;
            KeyHandler.OnUnmute -= OnUnmute;
        }

        public static void OnMute(object sender, EventArgs args) => deactivatePlayer.Play();
        public static void OnUnmute(object sender, EventArgs args) => activatePlayer.Play();
    }
}