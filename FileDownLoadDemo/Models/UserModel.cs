using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTAuthDemo.Models;

[Table("Tbl_User")]
public class UserModel
{
    [Key]
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
