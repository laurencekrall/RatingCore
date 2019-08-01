using Google.Apis.Auth.OAuth2;
using Google.Cloud.Vision.V1;
using Grpc.Auth;
using System;

namespace RatingCore.GoogleCP
{
    public class ImageAnnotatorClientFactory : IImageAnnotatorClientFactory
    {
        public ImageAnnotatorClient GetClient()
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
    }
}
