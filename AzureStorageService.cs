using Azure.Storage.Blobs;

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
            containerName = _configuration["AzureStorage:ContainerName"];

            // get azure connection string
            string connectionString = _configuration["AzureStorage:ConnectionString"];

            //initialize azure blob client
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task<string> UploadFileAsync(IFormFile file, string filename)
        {
            //get client container name
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            var blobClient = containerClient.GetBlobClient(filename);

            //file i/o
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }
            //return URL for the saved file
            return blobClient.Uri.ToString();
        }
    }
}
