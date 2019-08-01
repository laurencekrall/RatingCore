namespace RatingCore.GoogleCP
{
    internal class GetSimilarProductsOptions
    {
        public GetSimilarProductsOptions()
        {
        }

        public string ComputeRegion { get; set; }
        public string ProjectID { get; set; }
        public string ProductSetId { get; set; }
        public string Filter { get; set; }
        public string ProductCategory { get; set; }
    }
}