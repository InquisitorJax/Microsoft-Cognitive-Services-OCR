using Microsoft.ProjectOxford.Vision;
using Xamarin.Forms;

namespace Xamarin.MCS.OCR
{
    public interface IVisionClientFactory
    {
        IVisionServiceClient Build();
    }

    public class VisionClientFactory : IVisionClientFactory
    {
        public IVisionServiceClient Build()
        {
            var keyProvider = DependencyService.Get<IApiKeyProvider>();
            string key = keyProvider.GetApiKey(ApiKeyType.ComputerVisionApi);

            //NOTE: west central us hard-coded for all trial api keys
            const string apiRoute = "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0";
            
            var client = new VisionServiceClient(key, apiRoute);

            return client;
        }
    }
}