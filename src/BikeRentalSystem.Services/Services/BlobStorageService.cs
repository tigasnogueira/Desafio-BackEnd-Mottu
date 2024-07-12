using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Shared.Configurations;
using Microsoft.Extensions.Configuration;

namespace BikeRentalSystem.RentalServices.Services;

public class BlobStorageService : BaseService, IBlobStorageService
{
    private readonly IConfiguration _configuration;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;

    public BlobStorageService(IConfiguration configuration, INotifier notifier) : base(notifier)
    {
        _configuration = configuration;

        var azureBlobSettings = new AzureBlobStorageSettings();
        _configuration.GetSection("AzureBlobStorageSettings").Bind(azureBlobSettings);

        if (string.IsNullOrEmpty(azureBlobSettings.ConnectionString))
        {
            throw new ArgumentNullException(nameof(azureBlobSettings.ConnectionString), "Azure Blob Storage connection string cannot be null or empty.");
        }

        _blobServiceClient = new BlobServiceClient(azureBlobSettings.ConnectionString);
        _containerName = azureBlobSettings.ContainerName;
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStream, true);

            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task<Stream> DownloadFileAsync(string fileName)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            var blobDownloadInfo = await blobClient.DownloadAsync();
            return blobDownloadInfo.Value.Content;
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task DeleteFileAsync(string fileName)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.DeleteIfExistsAsync();
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }
}
