using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace Xamarin.MCS.OCR
{
    public interface IVisionClientFactory
    {
        IVisionServiceClient Build();

        ICustomVisionClient BuildCustom();
    }

    public interface ICustomVisionClient
    {
        Task<List<string>> GetWordsFromImage(Stream imageStream);
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

        public ICustomVisionClient BuildCustom()
        {
            var keyProvider = DependencyService.Get<IApiKeyProvider>();
            string key = keyProvider.GetApiKey(ApiKeyType.ComputerVisionApi);

            //NOTE: west central us hard-coded for all trial api keys            
            var client = new CustomVisionClient(key, "westcentralus");

            return client;

        }
    }

    
}