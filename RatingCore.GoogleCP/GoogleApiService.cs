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
        public string GetSimilar()
        {
            var client = _factory.GetClient();
            var path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, System.AppDomain.CurrentDomain.RelativeSearchPath ?? "");

            Image image = Image.FromFile($"{path}\\maddoggy.jpg");
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
