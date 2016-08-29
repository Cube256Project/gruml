using System.IO;

namespace Common
{
    public static class FileSystemUtilities
    {
        public static string CreateTemporaryDirectory()
        {
            var tmp = Path.GetTempFileName();
            File.Delete(tmp);
            Directory.CreateDirectory(tmp);
            return tmp;
        }
    }
}
