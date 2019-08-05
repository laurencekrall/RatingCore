using Google.Apis.Storage.v1.Data;
using Google.Cloud.Vision.V1;
using System.Threading.Tasks;

namespace RatingCore.GoogleCP
{
    public interface IGoogleApiService
    {
        Task<ProductSearchResults> GetSimilar(byte[] base64Image);
        Task<bool> CreateNewRateable(Rateable item);
    }
}