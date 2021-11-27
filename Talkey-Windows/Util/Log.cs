using System.IO;

namespace Util {
    public class Log {
        private static readonly string ERROR_FILE = "error.log";
        private static readonly string LOG_FILE = "logs.log";

        public static void E(string message) => File.AppendAllText(ERROR_FILE, message);
        public static void I(string message) => File.AppendAllText(LOG_FILE, "[INFO] " + message + "\n");
        public static void V(string message) => File.AppendAllText(LOG_FILE, "[VERBOSE] " + message + "\n");
        public static void W(string message) => File.AppendAllText(LOG_FILE, "[WARNING] " + message + "\n");
    }
}
