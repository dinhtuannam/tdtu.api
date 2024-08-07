namespace TDTU.API.Models.RegularJobApplicationModel;

public class RegularJobSetStatusRequest : BaseEntityDto
{
	public Guid CompanyId { get;set; }
	public string StatusId { get; set; }
}
