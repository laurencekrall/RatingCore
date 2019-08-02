using Google.Cloud.Vision.V1;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RatingCore.GoogleCP
{
    public class GoogleApiService : IGoogleApiService
    {
        IImageAnnotatorClientFactory _factory;
        IGcpProjectInfo _projectInfo;

        public GoogleApiService(IImageAnnotatorClientFactory factory, IGcpProjectInfo projectInfo)
        {
            _factory = factory;
            _projectInfo = projectInfo;
        }
        public string GetSimilar(byte[] base64Image)
        {
            var client = _factory.GetClient();

            Image image = Image.FromBytes(base64Image);

            var opts = new GetSimilarProductsOptions()
            {
                ComputeRegion = _projectInfo.ComputeRegion,
                ProjectID = _projectInfo.ProjectID,
                ProductSetId = "1",
                Filter = "",
                ProductCategory = "packagedgoods-v1",
            };

            var productSearchParams = new ProductSearchParams
            {
                ProductSetAsProductSetName = new ProductSetName(opts.ProjectID,
                                                               opts.ComputeRegion,
                                                               opts.ProductSetId),
                ProductCategories = { opts.ProductCategory },
                Filter = opts.Filter
            };

            // Search products similar to the image.
            var results = client.DetectSimilarProducts(image, productSearchParams);

            return JsonConvert.SerializeObject(results);
        }
    }


}
