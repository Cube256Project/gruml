using System.IO;

namespace Common
{
    public static class StreamExtensions
    {
        public static void Write(this Stream stream, byte[] data)
        {
            stream.Write(data, 0, data.Length);
        }

        public static Stream ToSeekableStream(this Stream s)
        {
            if (!s.CanSeek)
            {
                var ms = new MemoryStream();
                s.CopyTo(ms);
                ms.Position = 0;
                return ms;
            }
            else
            {
                return s;
            }
        }
    }
}
