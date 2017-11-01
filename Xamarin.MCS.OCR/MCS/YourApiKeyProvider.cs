using System;

namespace Xamarin.MCS.OCR
{
    public class YourApiKeyProvider : IApiKeyProvider
    {
        public string GetApiKey(ApiKeyType keyType)
        {
            string key = null;

            switch (keyType)
            {
                case ApiKeyType.ComputerVisionApi:
                    //TODO: Get your key here: https://www.microsoft.com/cognitive-services/en-us/computer-vision-api
                    key = "<COMPUTER_VISION_API_KEY>";
                    break;

                default:
                    throw new NotSupportedException("Cognitive service not supported!");
            }

            return key;
        }
    }
}