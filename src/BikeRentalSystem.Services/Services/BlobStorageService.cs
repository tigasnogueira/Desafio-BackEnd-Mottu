using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BikeRentalSystem.Core.Interfaces.Notifications;
using BikeRentalSystem.Core.Interfaces.Services;
using BikeRentalSystem.Shared.Configurations;
using Microsoft.Extensions.Options;

namespace BikeRentalSystem.RentalServices.Services;

public class BlobStorageService : BaseService, IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;

    public BlobStorageService(IOptions<AzureBlobStorageSettings> azureBlobSettings, INotifier notifier) : base(notifier)
    {
        if (string.IsNullOrEmpty(azureBlobSettings.Value.ConnectionString))
        {
            throw new ArgumentNullException(nameof(azureBlobSettings.Value.ConnectionString), "Azure Blob Storage connection string cannot be null or empty.");
        }

        _blobServiceClient = new BlobServiceClient(azureBlobSettings.Value.ConnectionString);
        _containerName = azureBlobSettings.Value.ContainerName ?? throw new ArgumentNullException(nameof(azureBlobSettings.Value.ContainerName), "Container name cannot be null or empty.");
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStream, true);

            _notifier.Handle($"File '{fileName}' uploaded successfully.");
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

            _notifier.Handle($"File '{fileName}' downloaded successfully.");
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

            _notifier.Handle($"File '{fileName}' deleted successfully.");
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }
}
