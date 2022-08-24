namespace Vendr.Contrib.Editors.FileUpload.Dtos;

public class DeleteFileDto
{
    public Guid StoreId { get; set; }
    public Guid OrderId { get; set; }
    public string Alias { get; set; }
    public string FileName { get; set; }
}