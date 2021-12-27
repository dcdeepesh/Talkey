using System.IO;

namespace Util {
    public class Log {
        const string ERROR_FILE = "LogError.log";
        const string LOG_FILE = "Log.log";

        public static void Init() {
            File.Delete(ERROR_FILE);
            File.Delete(LOG_FILE);
        }

        public static void E(string message) => File.AppendAllText(ERROR_FILE, $"{message}\n");
        public static void I(string message) => File.AppendAllText(LOG_FILE, $"[INFO] {message}\n");
        public static void V(string message) => File.AppendAllText(LOG_FILE, $"[VERBOSE] {message}\n");
        public static void W(string message) => File.AppendAllText(LOG_FILE, $"[WARN] {message}\n");
    }
}
