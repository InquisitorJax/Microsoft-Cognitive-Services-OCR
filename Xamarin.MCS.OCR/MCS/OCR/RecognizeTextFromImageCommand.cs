using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System.IO;
using System.Linq;
using System.Text;
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

            try
            {
                request.ImageStream.Seek(0, SeekOrigin.Begin);
                using (var stream = request.ImageStream)
                {
                    if (request.UseHandwriting)
                    {
                        var hwOps = await client.CreateHandwritingRecognitionOperationAsync(stream);
                        var result = await client.GetHandwritingRecognitionOperationResultAsync(hwOps);
                        retResult.TextResults = new List<string>();
                        do
                        {
                            if (result.RecognitionResult == null)
                            {
                                retResult.Notification.Add("Invalid result returned");
                                break;
                            }
                            var wordCollection = from line in result.RecognitionResult.Lines
                                from word in line.Words
                                select word.Text;

                            retResult.TextResults.AddRange(wordCollection.ToList());
                            result = await client.GetHandwritingRecognitionOperationResultAsync(hwOps);

                        } while (result.Status == HandwritingRecognitionOperationStatus.Running);

                        if (result.Status == HandwritingRecognitionOperationStatus.Failed)
                        {
                            retResult.Notification.Add(result.Status.ToString());
                        }
                    }
                    else
                    {
                        text = await client.RecognizeTextAsync(stream);
                        var wordCollection = from region in text.Regions
                            from line in region.Lines
                            from word in line.Words
                            select word.Text;

                        retResult.TextResults = wordCollection.ToList();
                    }                    
                }
            }
            catch (ClientException ex)
            {
                retResult.Notification.Add("Error calling cognitive services: " + ex.Error.Message);
            }

            return retResult;
        }
    }

    public class ImageRecognitionRequest
    {
        public ImageRecognitionRequest(Stream imageStream, bool useHandwriting = false)
        {
            ImageStream = imageStream;
            UseHandwriting = useHandwriting;
        }

        public Stream ImageStream { get; private set; }
        public bool UseHandwriting { get; private set; }
    }

    public class TextRecognitionResult : CommandResult
    {
        public List<string> TextResults { get; set; }
    }
}