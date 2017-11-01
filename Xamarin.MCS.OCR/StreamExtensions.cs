using System.IO;

namespace Xamarin.MCS.OCR
{
    public static class StreamExtensions
    {
        public static byte[] ToByteArray(this Stream stream)
        {
            byte[] retBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                retBytes = ms.ToArray();
            }

            return retBytes;
        }
    }
}