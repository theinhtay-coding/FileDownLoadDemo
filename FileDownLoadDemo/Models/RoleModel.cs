using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTAuthDemo.Models;

[Table("Tbl_Role")]
public class RoleModel
{
    [Key]
    public int RoleId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
