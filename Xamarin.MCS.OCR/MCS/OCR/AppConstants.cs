using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.MCS.OCR
{

    //from: https://github.com/Azure-Samples/cognitive-services-xamarin-forms-computer-vision-search/blob/master/VisualSearchApp/AppConstants.cs
    public static class AppConstants
    {
        #region bing web search api
        /// <summary>
        /// Header parameter used to provide the authentication key for all API calls
        /// </summary>
        public const string OcpApimSubscriptionKey = "Ocp-Apim-Subscription-Key";

        #endregion

        #region computer vision api
        /// <summary>
        /// Url of the Computer Vision API OCR method for printed text
        /// [language=en] Text in image is in English. 
        /// [detectOrientation=true] Improve results by detecting orientation
        /// </summary>
        public static string ComputerVisionApiOcrUrl = "";

        /// <summary>
        /// Url of the Computer Vision API handwritten text recognition method
        /// [handwriting=true] Text in image is handwritten. Set to false for printed text.
        /// </summary>
		public static string ComputerVisionApiHandwritingUrl = "";

        public static void SetOcrLocation(string location)
        {
            ComputerVisionApiOcrUrl = $"https://{location}.api.cognitive.microsoft.com/vision/v1.0/ocr?language=en&detectOrientation=true";
            ComputerVisionApiHandwritingUrl = $"https://{location}.api.cognitive.microsoft.com/vision/v1.0/recognizeText?handwriting=true";
        }

        #endregion
    }
}
