using Microsoft.ProjectOxford.Vision.Contract;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wibci.LogicCommand;
using Xamarin.Forms;

namespace Xamarin.MCS.OCR.MCS.OCR
{
    public interface IRecognizeTextFromImageCommand : IAsyncLogicCommand<TextFromImageRecognitionRequest, TextRecognitionResult>
    {
    }

    public class RecognizeTextFromImageCommand : AsyncLogicCommand<TextFromImageRecognitionRequest, TextRecognitionResult>, IRecognizeTextFromImageCommand
    {
        private IVisionClientFactory VisionClientFactory
        {
            get { return DependencyService.Get<IVisionClientFactory>(); }
        }

        public override async Task<TextRecognitionResult> ExecuteAsync(TextFromImageRecognitionRequest request)
        {
            TextRecognitionResult retResult = new TextRecognitionResult();
            OcrResults text;

            var client = VisionClientFactory.Build();

            using (var stream = request.ImageStream)
                text = await client.RecognizeTextAsync(stream);

            var wordCollection = from region in text.Regions
                                 from line in region.Lines
                                 from word in line.Words
                                 select word.Text;

            retResult.TextResults = wordCollection.ToArray();

            return retResult;
        }
    }

    public class TextFromImageRecognitionRequest
    {
        public TextFromImageRecognitionRequest(Stream imageStream)
        {
            ImageStream = imageStream;
        }

        public Stream ImageStream { get; private set; }
    }

    public class TextRecognitionResult : CommandResult
    {
        public string[] TextResults { get; set; }
    }
}