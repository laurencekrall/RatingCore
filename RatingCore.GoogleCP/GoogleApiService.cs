﻿using Google.Cloud.Vision.V1;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Object = Google.Apis.Storage.v1.Data.Object;
using System.Threading.Tasks;

namespace RatingCore.GoogleCP
{
    public class GoogleApiService : IGoogleApiService
    {
        IClientFactory _factory;
        IGcpProjectInfo _projectInfo;

        public GoogleApiService(IClientFactory factory, IGcpProjectInfo projectInfo)
        {
            _factory = factory;
            _projectInfo = projectInfo;
        }

        public async Task<Object> AddImageToBucket(byte[] base64Image, string name)
        {
            var client = _factory.CreateStorageClient();

            Stream stream = new MemoryStream(base64Image);

            var res = await client.UploadObjectAsync(_projectInfo.BucketName, name, null, stream);

            return res;
        }

        public async Task<ProductSearchResults> GetSimilar(byte[] base64Image)
        {
            var client = _factory.CreateImageAnnotatorClient();

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
            var results = await client.DetectSimilarProductsAsync(image, productSearchParams);

            return results;
        }
    }


}
