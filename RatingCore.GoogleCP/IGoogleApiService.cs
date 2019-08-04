using Google.Apis.Storage.v1.Data;
using Google.Cloud.Vision.V1;
using System.Threading.Tasks;

namespace RatingCore.GoogleCP
{
    public interface IGoogleApiService
    {
        Task<ProductSearchResults> GetSimilar(byte[] base64Image);
        Task<Object> AddImageToBucket(byte[] base64Image, string fileName);
        Task<Product> CreateProduct(byte[] base64Image, string productName);
        Task<ReferenceImage> AddImageToProduct(byte[] base64Image, string fileName,
                                string productName, string productID, string imageURL, string referenceImageID);
    }
}