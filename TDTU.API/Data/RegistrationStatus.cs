using System.ComponentModel.DataAnnotations.Schema;

namespace TDTU.API.Data;

[Table("tb_registration_status")]
public class RegistrationStatus : BaseStatusEntity
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public ICollection<InternshipRegistration>? Registrations { set; get; }
}
