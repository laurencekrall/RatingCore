using Google.Cloud.Vision.V1;

namespace RatingCore.GoogleCP
{
    public interface IImageAnnotatorClientFactory
    {
        ImageAnnotatorClient GetClient();
    }
}