using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Vision.Contract;
using Newtonsoft.Json.Linq;

namespace Xamarin.MCS.OCR
{
    public class CustomVisionClient : ICustomVisionClient
    {

        private readonly HttpClient _client;

        public CustomVisionClient(string key, string location)
        {
            AppConstants.SetOcrLocation(location);
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add(AppConstants.OcpApimSubscriptionKey, key);
        }


        public async Task<OcrResults> RecognizeTextAsync(Stream imageStream, string languageCode = "unk", bool detectOrientation = true)
        {
            //TODO: FINISH implementation of IVisionServiceClient

            HttpResponseMessage response = null;
            byte[] image = imageStream.ToByteArray();
            using (var content = new ByteArrayContent(image))
            {
                // The media type of the body sent to the API. 
                // "application/octet-stream" defines an image represented 
                // as a byte array
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await _client.PostAsync(AppConstants.ComputerVisionApiOcrUrl, content);
            }

            string responseString = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(responseString);

            OcrResults results = new OcrResults();

            List<Region> regionItems = new List<Region>();

            // Here, we pull down each "line" of text and then join it to 
            // make a string representing the entirety of each line.  
            // In the Handwritten endpoint, you are able to extract the 
            // "line" without any further processing.  If you would like 
            // to simply get a list of all extracted words,* you can do 
            // this with:
            //
            // json.SelectTokens("$.regions[*].lines[*].words[*].text) 
            IEnumerable<JToken> regions = json.SelectTokens("$.regions[*]");
            if (regions != null)
            {
                foreach (JToken region in regions)
                {
                    var regionItem = new Region { BoundingBox = region.SelectToken($".boundingBox").ToString() };
                    IEnumerable<JToken> lines = region.SelectTokens("$.lines[*]");
                    if (lines != null)
                    {
                        foreach (JToken line in lines)
                        {
                            IEnumerable<JToken> words = line.SelectTokens("$.words[*].text");
                            if (words != null)
                            {
                                //                                wordList.Add(string.Join(" ", words.Select(x => x.ToString())));
                            }
                        }
                    }
                    regionItems.Add(regionItem);
                }
            }

            return results;

        }

        public async Task<List<string>> GetWordsFromImage(Stream imageStream)
        {
            List<string> wordList = new List<string>();
            if (imageStream != null)
            {
                HttpResponseMessage response = null;
                using (var content = new ByteArrayContent(imageStream.ToByteArray()))
                {
                    // The media type of the body sent to the API. 
                    // "application/octet-stream" defines an image represented 
                    // as a byte array
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response = await _client.PostAsync(AppConstants.ComputerVisionApiOcrUrl, content);
                }

                string responseString = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(responseString);

                // Here, we pull down each "line" of text and then join it to 
                // make a string representing the entirety of each line.  
                // In the Handwritten endpoint, you are able to extract the 
                // "line" without any further processing.  If you would like 
                // to simply get a list of all extracted words,* you can do 
                // this with:
                //
                // json.SelectTokens("$.regions[*].lines[*].words[*].text) 
                IEnumerable<JToken> lines = json.SelectTokens("$.regions[*].lines[*]");
                if (lines != null)
                {
                    foreach (JToken line in lines)
                    {
                        IEnumerable<JToken> words = line.SelectTokens("$.words[*].text");
                        if (words != null)
                        {
                            wordList.Add(string.Join(" ", words.Select(x => x.ToString())));
                        }
                    }
                }
            }
            return wordList;
        }
    }
}
