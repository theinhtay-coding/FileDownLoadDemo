using FileDownLoadDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.IO.Compression;

namespace FileDownLoadDemo.ApiControllers;

[Route("api/[controller]")]
[ApiController]
public class FileController : ControllerBase
{
    [HttpGet("Download/{fileName}")]
    public IActionResult GetFileByName(string fileName)
    {
        // Combine the directory with the file name to get the full file path
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", fileName);

        // Check if the file exists
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        // Get the content type for the file
        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(fileName, out var contentType))
        {
            contentType = "application/octet-stream"; // Default content type if the type cannot be determined
        }

        // Return the file with the appropriate content type and ensure it is downloaded
        return File(System.IO.File.ReadAllBytes(filePath), contentType, fileName);
    }

    [HttpGet("DownloadAll")]
    public IActionResult DownloadAll()
    {
        // Define the list of files to include in the ZIP archive
        var files = new List<FileModel>
                    {
                        new FileModel { FileName = "javabook.pdf", FilePath = Path.Combine("wwwroot", "Files", "javabook.pdf") },
                        new FileModel { FileName = "csharpbook.pdf", FilePath = Path.Combine("wwwroot", "Files", "csharpbook.pdf") }
                    };

        var memoryStream = new MemoryStream();

        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            foreach (var file in files)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), file.FilePath);

                if (System.IO.File.Exists(filePath))
                {
                    var fileBytes = System.IO.File.ReadAllBytes(filePath);
                    var zipEntry = archive.CreateEntry(file.FileName, CompressionLevel.Fastest);

                    using (var zipStream = zipEntry.Open())
                    {
                        zipStream.Write(fileBytes, 0, fileBytes.Length);
                    }
                }
                else
                {
                    // Handle missing file case if necessary
                }
            }
        }

        // Reset the position of the memoryStream to the beginning
        memoryStream.Position = 0;

        // Generate a file name with a timestamp
        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        var fileName = $"AllFiles_{timestamp}.zip";

        // Return the file as a ZIP download
        return File(memoryStream, "application/zip", fileName);
    }
}
