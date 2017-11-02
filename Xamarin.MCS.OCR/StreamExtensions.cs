using System.IO;
using System.Threading.Tasks;
using Wibci.Xamarin.Images;
using Xamarin.Forms;

namespace Xamarin.MCS.OCR
{
    public static class StreamExtensions
    {
        public static byte[] ToByteArray(this Stream stream)
        {
            byte[] retBytes;
            stream.Seek(0, SeekOrigin.Begin);
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                retBytes = ms.ToArray();
            }

            return retBytes;
        }

        public static async Task<Stream> ResizeAsync(this Stream stream, int dimension)
        {
            if (stream == null || stream.Length == 0)
                return stream;

            var resizeCommand = DependencyService.Get<IResizeImageCommand>();

            byte[] original = stream.ToByteArray();
            var request = new ResizeImageContext
                          {
                              Height = dimension,
                              Width = dimension,
                              OriginalImage = original
                          };
            var result = await resizeCommand.ExecuteAsync(request);

            var retStream = new MemoryStream(result.ResizedImage);

            return retStream;

        }
    }
}