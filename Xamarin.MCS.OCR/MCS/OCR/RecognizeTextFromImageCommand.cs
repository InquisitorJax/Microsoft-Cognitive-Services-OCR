using System.Diagnostics;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wibci.LogicCommand;
using Xamarin.Forms;

namespace Xamarin.MCS.OCR
{
    public interface IRecognizeTextFromImageCommand : IAsyncLogicCommand<ImageRecognitionRequest, TextRecognitionResult>
    {
    }

    public class RecognizeTextFromImageCommand : AsyncLogicCommand<ImageRecognitionRequest, TextRecognitionResult>, IRecognizeTextFromImageCommand
    {
        private IVisionClientFactory VisionClientFactory
        {
            get { return DependencyService.Get<IVisionClientFactory>(); }
        }

        public override async Task<TextRecognitionResult> ExecuteAsync(ImageRecognitionRequest request)
        {
            TextRecognitionResult retResult = new TextRecognitionResult();
            OcrResults text = null;

            var client = VisionClientFactory.Build();

            bool success = true;
            try
            {
                using (var stream = request.ImageStream)
                {
                    text = await client.RecognizeTextAsync(stream);
//                    var hwOps = await client.CreateHandwritingRecognitionOperationAsync(stream);
//                    var result = await client.GetHandwritingRecognitionOperationResultAsync(hwOps);                    
                }
            }
            catch (ClientException ex)
            {
                success = false;
                retResult.Notification.Add("Error calling cognitive services: " + ex.Error.Message);
            }

            if (text != null && success)
            {
                var wordCollection = from region in text.Regions
                                     from line in region.Lines
                                     from word in line.Words
                                     select word.Text;

                retResult.TextResults = wordCollection.ToArray();
            }

            return retResult;
        }
    }

    public class ImageRecognitionRequest
    {
        public ImageRecognitionRequest(Stream imageStream)
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