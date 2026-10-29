using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace MoviesAdmin.Services 
{
    public class AzureStorageService : IAzureStorageService
    {
        private readonly IConfiguration _configuration;

        private readonly BlobServiceClient _blobServiceClient;
        //configuration is used to get appsettings, user-secrets, env vars, etc.
        private string containerName;

        //constructor
        public AzureStorageService(IConfiguration configuration)
        {
            _configuration = configuration;

            //get azure container name
            containerName = _configuration["AzureStorage:ContainerName"] ?? throw new InvalidOperationException("Missing configuration: AzureStorage:ContainerName");

            // get azure connection string
            string connectionString = _configuration["AzureStorage:ConnectionString"] ?? throw new InvalidOperationException("Missing configuration: AzureStorage:ConnectionString");

            //initialize azure blob client
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task<string> UploadFileAsync(IFormFile file, string fileName)
        {
            //get client container name
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            var blobClient = containerClient.GetBlobClient(fileName);

            var options = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                }
            };

            //file i/o
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, options); // pass in options when uploading
            }
            //return URL for the saved file
            return blobClient.Uri.ToString();
        }
    }
}
