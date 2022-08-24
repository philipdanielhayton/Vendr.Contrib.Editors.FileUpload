using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Vendr.Contrib.Editors.FileUpload.Config;
using Vendr.Contrib.Editors.FileUpload.Dtos;

namespace Vendr.Contrib.Editors.FileUpload.Services;

public class VendrOrderUploadService : IVendrOrderUploadService
{
    private readonly IWebHostEnvironment _env;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IOptions<VendrFileUploadOptions> _options;

    public VendrOrderUploadService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor, IOptions<VendrFileUploadOptions> options)
    {
        _env = env;
        _httpContextAccessor = httpContextAccessor;
        _options = options;
    }

    public string GetFolderPath(Guid storeId, Guid orderId, string alias) => $"{_options.Value.RootFolder}/{storeId:D}/{orderId:D}/{alias}";
    public string GetFilePath(Guid storeId, Guid orderId, string alias, string fileName) => $"{GetFolderPath(storeId, orderId, alias)}/{fileName}";

    public async Task Save(Guid storeId, Guid orderId, string alias, IFormFile file)
    {
        //  Create folder on disk
        Directory.CreateDirectory(Path.Combine(_env.WebRootPath, GetFolderPath(storeId, orderId, alias)));

        var filePath = GetFilePath(storeId, orderId, alias, file.FileName);
        await using var fs = new FileStream(Path.Combine(_env.WebRootPath, filePath), FileMode.Create);
        await file.CopyToAsync(fs);
    }

    public IEnumerable<VendrFileInfo> GetFiles(Guid storeId, Guid orderId, string alias)
    {
        var request = _httpContextAccessor.HttpContext?.Request;

        if (request == null)
            throw new InvalidOperationException("Request must be available to save ticket");

        var filePaths = Directory.GetFiles(Path.Combine(_env.WebRootPath, GetFolderPath(storeId, orderId, alias)));

        return filePaths
            .Select(filePath =>
                new FileInfo(filePath)).Select(file => new VendrFileInfo
            {
                Name = file.Name,
                Url = $"{request.Scheme}://{request.Host}/{GetFilePath(storeId, orderId, alias, file.Name)}"
            });
    }

    public void Delete(Guid storeId, Guid orderId, string alias, string fileName)
    {
        File.Delete(Path.Combine(_env.WebRootPath, GetFilePath(storeId, orderId, alias, fileName)));
    }
}