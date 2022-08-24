using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Vendr.Contrib.Editors.FileUpload.Dtos;
using Vendr.Contrib.Editors.FileUpload.Services;

namespace Vendr.Contrib.Editors.FileUpload.Controllers;

public class FileUploadController : UmbracoAuthorizedApiController
{
    private readonly IVendrOrderUploadService _uploadService;

    public FileUploadController(IVendrOrderUploadService uploadService)
    {
        _uploadService = uploadService;
    }

    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] Guid orderId, [FromForm] Guid storeId, [FromForm] string alias)
    {
        if (!Request.HasFormContentType)
            return BadRequest("No files submitted");

        foreach (var file in Request.Form.Files)
        {
            if (file.Length <= 0) continue;
            await _uploadService.Save(storeId, orderId, alias, file);
        }

        return new JsonResult(_uploadService.GetFiles(storeId, orderId, alias));
    }

    [HttpPost]
    public IActionResult Delete(DeleteFileDto model)
    {
        _uploadService.Delete(model.StoreId, model.OrderId, model.Alias, model.FileName);
        return new JsonResult(_uploadService.GetFiles(model.StoreId, model.OrderId, model.Alias));
    }
}