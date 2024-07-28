using FileDownLoadDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace FileDownLoadDemo.Controllers;

public class FileManagementController : Controller
{
    private readonly AppDbContext _context;

    public FileManagementController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        //var files = await _context.Files.ToListAsync();
        var files = await _context.Files
        .Select(f => new
        {
            Id = f.Id,
            FileName = f.FileName
        })
        .ToListAsync();
        var model = files.Select(f => new FileEntity { Id = f.Id, FileName = f.FileName }).ToList();
        return View(model);
    }

    [HttpGet]
    public IActionResult Upload()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            TempData["UploadMessage"] = "No file selected or file is empty.";
            TempData["UploadSuccess"] = false;
            return RedirectToAction(nameof(Index));
            //return BadRequest("No file selected or file is empty");
        }

        var fileEntity = new FileEntity
        {
            FileName = file.FileName
        };

        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            fileEntity.FileContent = memoryStream.ToArray();
        }

        _context.Add(fileEntity);
        await _context.SaveChangesAsync();

        TempData["UploadMessage"] = "File uploaded successfully.";
        TempData["UploadSuccess"] = true;
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Download(int id)
    {
        var file = await _context.Files.FindAsync(id);

        if (file == null)
        {
            return NotFound();
        }

        return File(file.FileContent, "application/pdf; charset=utf-8", file.FileName);
    }

    [HttpGet]
    public async Task<IActionResult> DownloadAll()
    {
        var files = await _context.Files.ToListAsync();

        var memoryStream = new MemoryStream();

        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            foreach (var file in files)
            {
                var zipEntry = archive.CreateEntry(file.FileName, CompressionLevel.Fastest);
                using (var zipStream = zipEntry.Open())
                {
                    zipStream.Write(file.FileContent, 0, file.FileContent.Length);
                }
            }
        }

        memoryStream.Position = 0;

        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        var fileName = $"AllFiles_{timestamp}.zip";

        return File(memoryStream, "application/zip", fileName);
    }
}
