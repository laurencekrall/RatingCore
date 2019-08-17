using Google.Apis.Storage.v1.Data;
using Google.Cloud.Vision.V1;
using RatingCore.GoogleCP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RatingCore.GoogleCP
{
    public interface IGoogleApiService
    {
        Task<List<ProductSearchResult>> GetSimilarAsync(byte[] base64Image);
        Task<bool> CreateNewRateable(Rateable item);
    }
}