using TDTU.API.Dtos.RegistrationStatusDTO;

namespace TDTU.API.Dtos.InternshipRegistrationDTO;

public class InternshipRegistrationDto : BaseEntityDto
{
	public string Code { get; set; }
	public string FullName { get; set; }
	public string Position { get; set; }
	public string StatusId { get; set; }
	public RegistrationStatusDto Status { get; set; }
}
