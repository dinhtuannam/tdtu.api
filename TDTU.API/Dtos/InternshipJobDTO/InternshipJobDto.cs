using AutoMapper;
using TDTU.API.Dtos.CompanyDTO;

namespace TDTU.API.Dtos.InternshipJobDTO;

public class InternshipJobDto : BaseEntityDto
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public Guid CompanyId { get; set; }
	public CompanyDto Company { get; set; }
	public Guid InternshipTermId { get; set; }
	public DateTime StartDate { get; set; } = DateTime.Now;
	public DateTime EndDate { get; set; } = DateTime.Now.AddMonths(3);
	private class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<InternshipJob, InternshipJobDto>()
				.ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company))
				.ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.InternshipTerm!.StartDate))
				.ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.InternshipTerm!.EndDate));
		}
	}
}
