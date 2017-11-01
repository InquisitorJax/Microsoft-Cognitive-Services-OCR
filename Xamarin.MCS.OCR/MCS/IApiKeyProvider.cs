namespace Xamarin.MCS.OCR
{
    public enum ApiKeyType
    {
        FaceApi,
        ComputerVisionApi
    }

    public interface IApiKeyProvider
    {
        string GetApiKey(ApiKeyType keyType);
    }
}