using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Vendr.Contrib.Editors.FileUpload.Config;
using Vendr.Contrib.Editors.FileUpload.Services;

namespace Vendr.Contrib.Editors.FileUpload;

public class Composer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.Services.Configure<VendrFileUploadOptions>(builder.Config.GetSection(Constants.ConfigSectionName));
        builder.Services.AddScoped<IVendrOrderUploadService, VendrOrderUploadService>();
    }
}