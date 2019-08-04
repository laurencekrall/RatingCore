using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Google.Cloud.Vision.V1;
using Grpc.Auth;
using System;

namespace RatingCore.GoogleCP
{
    public class ClientFactory : IClientFactory
    {
        public ImageAnnotatorClient CreateImageAnnotatorClient()
        {
            var path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, System.AppDomain.CurrentDomain.RelativeSearchPath ?? "");
            var credential = GoogleCredential.FromFile($"{path}\\token.json")
               .CreateScoped(ImageAnnotatorClient.DefaultScopes);
            var channel = new Grpc.Core.Channel(
                ImageAnnotatorClient.DefaultEndpoint.ToString(),
                credential.ToChannelCredentials());

            var imageAnnotatorClient = ImageAnnotatorClient.Create(channel);

            return imageAnnotatorClient;
        }

        public ProductSearchClient CreateProductSearchClient()
        {
            var path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, System.AppDomain.CurrentDomain.RelativeSearchPath ?? "");
            var credential = GoogleCredential.FromFile($"{path}\\token.json")
               .CreateScoped(ProductSearchClient.DefaultScopes);
            var channel = new Grpc.Core.Channel(
                ProductSearchClient.DefaultEndpoint.ToString(),
                credential.ToChannelCredentials());

            var imageAnnotatorClient = ProductSearchClient.Create(channel);

            return imageAnnotatorClient;
        }

        public StorageClient CreateStorageClient()
        {
            var path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, System.AppDomain.CurrentDomain.RelativeSearchPath ?? "");
            var credential = GoogleCredential.FromFile($"{path}\\token.json")
               .CreateScoped(ImageAnnotatorClient.DefaultScopes);
            
            var storageClient = StorageClient.Create(credential);

            return storageClient;
        }
    }
}
