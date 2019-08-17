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
using RatingCore.GoogleCP.Models;
using System.Linq;

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

            var res = await client.UploadObjectAsync(_projectInfo.BucketName, name, "image/png", stream);

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

        public async Task<List<ProductSearchResult>> GetSimilarAsync(byte[] base64Image)
        {
            var client = _clientFactory.CreateImageAnnotatorClient();
            var productClient = _clientFactory.CreateProductSearchClient();

            Image image = Image.FromBytes(base64Image);

            var productSearchParams = new ProductSearchParams
            {
                ProductSetAsProductSetName = new ProductSetName(_projectInfo.ProjectID,
                                                               _projectInfo.ComputeRegion,
                                                               "1"),
                ProductCategories = { "packagedgoods-v1" },
                Filter = ""
            };

            // Search products similar to the image.
            var detectSimilar = await client.DetectSimilarProductsAsync(image, productSearchParams);

            var mapped =  detectSimilar.Results.Select(async x => 
            {
                var item = new ProductSearchResult()
                {
                    Score = x.Score,
                    ProductName = x.Product.Name.Split("/").Last(),
                };

                if (x.Score > 0.6)
                {
                    item.ReferenceImages = (await ListReferenceImagesOfProduct(productClient, item.ProductName)).ToList();
                }
                return item;
            }).ToList();

            var results = await Task.WhenAll(mapped);

            return results.ToList();
        }

        private async Task<IEnumerable<string>> ListReferenceImagesOfProduct(ProductSearchClient client, string productID)
        {
            var request = new ListReferenceImagesRequest
            {
                ParentAsProductName = new ProductName(_projectInfo.ProjectID,
                                                      _projectInfo.ComputeRegion,
                                                      productID)
            };

            var res = client.ListReferenceImagesAsync(request);

            var results = await res.ToList();

            return results.Select(x => x.Uri.Replace("gs://", "https://storage.cloud.google.com/"));            
        }
    }


}
