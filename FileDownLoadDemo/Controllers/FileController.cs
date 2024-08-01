using FileDownLoadDemo.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.IO.Compression;

namespace FileDownLoadDemo.Controllers;

public class FileController : Controller
{
    public IActionResult Index()
    {
        // Create a list of files (this can come from a database or other source)
        var files = new List<FileModel>
        {
            new FileModel { FileName = "javabook.pdf", FilePath = Path.Combine("Files", "javabook.pdf") },
            new FileModel { FileName = "csharpbook.pdf", FilePath = Path.Combine("Files", "csharpbook.pdf") },
            new FileModel { FileName = "bpf.jpeg", FilePath = Path.Combine("Files", "bpf.jpeg") },
            new FileModel { FileName = "gpf.jpeg", FilePath = Path.Combine("Files", "gpf.jpeg") },
            new FileModel { FileName = "Lab.doc", FilePath = Path.Combine("Files", "Lab.doc") },
            new FileModel { FileName = "User.xlsx", FilePath = Path.Combine("Files", "User.xlsx") }
        };

        return View(files);
    }

    [HttpGet]
    public IActionResult Download(string fileName)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", fileName);

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        var memory = new MemoryStream();
        using (var stream = new FileStream(filePath, FileMode.Open))
        {
            stream.CopyTo(memory);
        }

        memory.Position = 0;

        return File(memory, "application/pdf; charset=utf-8", fileName);
    }

    [HttpGet]
    public IActionResult DownloadAll()
    {
        var files = new List<FileModel>
            {
                new FileModel { FileName = "javabook.pdf", FilePath = Path.Combine("wwwroot", "Files", "javabook.pdf") },
                new FileModel { FileName = "csharpbook.pdf", FilePath = Path.Combine("wwwroot", "Files", "csharpbook.pdf") },
                new FileModel { FileName = "bpf.jpeg", FilePath = Path.Combine("wwwroot", "Files", "bpf.jpeg") },
                new FileModel { FileName = "gpf.jpeg", FilePath = Path.Combine("wwwroot", "Files", "gpf.jpeg") },
                new FileModel { FileName = "Lab.doc", FilePath = Path.Combine("wwwroot", "Files", "Lab.doc") },
                new FileModel { FileName = "User.xlsx", FilePath = Path.Combine("wwwroot", "Files", "User.xlsx") }
            };

        var memoryStream = new MemoryStream();

        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            foreach (var file in files)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), file.FilePath);
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                var zipEntry = archive.CreateEntry(file.FileName, CompressionLevel.Fastest);
                using (var zipStream = zipEntry.Open())
                {
                    zipStream.Write(fileBytes, 0, fileBytes.Length);
                }
            }
        }

        // Reset the position of the memoryStream to the beginning
        memoryStream.Position = 0;

        // Generate file name with timestamp
        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        var fileName = $"AllFiles_{timestamp}.zip";

        // Return the file with the generated name
        return File(memoryStream, "application/zip", fileName);
    }
}
