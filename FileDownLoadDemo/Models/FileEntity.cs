using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileDownLoadDemo.Models;

[Table("Tbl_File")]
public class FileEntity
{
    [Key]
    public int Id { get; set; }
    public string FileName { get; set; }
    public byte[] FileContent { get; set; }
}
