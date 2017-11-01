namespace Xamarin.MCS.OCR.MCS
{
    public enum ApiKeyType
    {
        ComputerVisionApi
    }

    public interface IApiKeyProvider
    {
        string GetApiKey(ApiKeyType keyType);
    }
}