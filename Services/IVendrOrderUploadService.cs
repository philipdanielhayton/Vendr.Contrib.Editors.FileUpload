using Microsoft.AspNetCore.Http;
using Vendr.Contrib.Editors.FileUpload.Dtos;

namespace Vendr.Contrib.Editors.FileUpload.Services;

public interface IVendrOrderUploadService
{
    public string GetFolderPath(Guid storeId, Guid orderId, string alias);
    public string GetFilePath(Guid storeId, Guid orderId, string alias, string fileName);
    public Task Save(Guid storeId, Guid orderId, string alias, IFormFile file);
    public IEnumerable<VendrFileInfo> GetFiles(Guid storeId, Guid orderId, string alias);
    public void Delete(Guid storeId, Guid orderId, string alias, string fileName);
}