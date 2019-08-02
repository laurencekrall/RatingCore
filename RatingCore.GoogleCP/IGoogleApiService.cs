namespace RatingCore.GoogleCP
{
    public interface IGoogleApiService
    {
        string GetSimilar(byte[] base64Image);
    }
}