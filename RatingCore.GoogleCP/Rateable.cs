namespace RatingCore.GoogleCP
{
    public class Rateable
    {
        public string ProductName { get; set; }
        public byte[] Base64Image { get; set; }
        public string FileName { get; set; }
    }
}