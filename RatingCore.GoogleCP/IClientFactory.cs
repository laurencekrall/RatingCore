using Google.Cloud.Storage.V1;
using Google.Cloud.Vision.V1;

namespace RatingCore.GoogleCP
{
    public interface IClientFactory
    {
        ImageAnnotatorClient CreateImageAnnotatorClient();
        StorageClient CreateStorageClient();
    }
}