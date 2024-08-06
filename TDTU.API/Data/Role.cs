using System.ComponentModel.DataAnnotations.Schema;

namespace TDTU.API.Data;

[Table("tb_roles")]
public class Role : BaseStatusEntity
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public ICollection<User>? Users { set; get; }
}

public static class RoleConstant
{
	public static string Student { get; } = nameof(Student).ToUpper();
	public static string Admin { get; } = nameof(Admin).ToUpper();
	public static string Company { get; } = nameof(Company).ToUpper();
}