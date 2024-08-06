using System.ComponentModel.DataAnnotations.Schema;

namespace TDTU.API.Data;

[Table("tb_application_status")]
public class ApplicationStatus : BaseStatusEntity
{
	public string Name { get; set; }
	public string Description { get; set; }
	public ICollection<InternshipJobApplication>? InternshipJobApplications { set; get; }
}
