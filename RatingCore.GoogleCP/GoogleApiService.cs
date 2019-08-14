using Google.Cloud.Vision.V1;
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
        IClientFactory _clientFactory;
        IGcpProjectInfo _projectInfo;

        public GoogleApiService(IClientFactory clientFactory, IGcpProjectInfo projectInfo)
        {
            _clientFactory = clientFactory;
            _projectInfo = projectInfo;
        }

        public async Task<bool> CreateNewRateable(Rateable item)
        {
            var addImage = await AddImageToBucket(item.Base64Image, item.FileName);
            var makeProduct = await CreateProduct(item.ProductName);
            var bucketFileLocation = $"gs://{_projectInfo.BucketName}/{item.FileName}";
            var combine = await AddImageToProduct(item.ProductName, bucketFileLocation, item.FileName);
            var addToSet = await AddProductToProductSet(item.ProductName);
            return true;
        }

        private async Task<Object> AddImageToBucket(byte[] base64Image, string name)
        {
            var client = _clientFactory.CreateStorageClient();

            Stream stream = new MemoryStream(base64Image);

            var res = await client.UploadObjectAsync(_projectInfo.BucketName, name, null, stream);

            return res;
        }

        private async Task<Product> CreateProduct(string productName)
        {
            var client = _clientFactory.CreateProductSearchClient();
            var request = new CreateProductRequest
            {
                ParentAsLocationName = new LocationName(_projectInfo.ProjectID,
                                                        _projectInfo.ComputeRegion),
                Product = new Product
                {
                    DisplayName = productName,
                    ProductCategory = "packagedgoods-v1"
                },
                ProductId = productName
            };

            // The response is the product with the `name` field populated.
            var product = await client.CreateProductAsync(request);

            return product;
        }

        private async Task<ReferenceImage> AddImageToProduct(string productID, string imageURL, string referenceImageID)
        {
            var client = _clientFactory.CreateProductSearchClient();

            var parent = new ProductName(_projectInfo.ProjectID,
                                                      _projectInfo.ComputeRegion,
                                                      productID);

            var refImage = new ReferenceImage
            {
                Uri = imageURL
            };

            var request = new CreateReferenceImageRequest
            {
                // Get the full path of the product.
                ParentAsProductName = parent,
                ReferenceImageId = referenceImageID,
                // Create a reference image.
                ReferenceImage = refImage
            };

            var referenceImage = await client.CreateReferenceImageAsync(request);
            return referenceImage;
        }

        private async Task<bool> AddProductToProductSet(string productID)
        {
            var client = _clientFactory.CreateProductSearchClient();

            var request = new AddProductToProductSetRequest
            {
                ProductAsProductName = new ProductName(_projectInfo.ProjectID,
                                                      _projectInfo.ComputeRegion,
                                                      productID),

                ProductSetName = new ProductSetName(_projectInfo.ProjectID,
                                                   _projectInfo.ComputeRegion,
                                                   "1"),
            };

            await client.AddProductToProductSetAsync(request);

            Console.WriteLine("Product added to product set.");

            return true;
        }

        public async Task<ProductSearchResults> GetSimilarAsync(byte[] base64Image)
        {
            var client = _clientFactory.CreateImageAnnotatorClient();

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
