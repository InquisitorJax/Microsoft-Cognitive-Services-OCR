using Microsoft.ProjectOxford.Vision;
using Xamarin.Forms;

namespace Xamarin.MCS.OCR.MCS.OCR
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

            var faceServiceClient = new VisionServiceClient(key);

            return faceServiceClient;
        }
    }
}