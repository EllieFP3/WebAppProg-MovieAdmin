namespace MoviesAdmin.Services
{
    public interface IAzureStorageService
    {
        //service to upload to azure blob storage
        //return: the URL of the uploaded file as a string
        public Task<string> UploadFileAsync(IFormFile file, string fileName);
        // TO-DO: add a function that deletes the image for a movie record on deletion
    }
}
