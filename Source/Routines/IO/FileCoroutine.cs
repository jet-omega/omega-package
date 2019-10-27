using System.IO;

namespace Omega.Routines.IO
{
    public static class FileRoutine
    {
        public static Routine WriteAllTextRoutine(string path, string text)
            => Routine.Task(() => File.WriteAllText(path, text));
        public static Routine<string> ReadAllTextRoutine(string path)
            => Routine.Task(() => File.ReadAllText(path));

        public static Routine WriteAllBytesRoutine(string path, byte[] data) =>
            Routine.Task(() => File.WriteAllBytes(path, data));
        public static Routine<byte[]> ReadAllBytesRoutine(string path) =>
            Routine.Task(() => File.ReadAllBytes(path));
    }
}