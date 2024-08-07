using AutoMapper;

namespace TDTU.API.Dtos.SkillDTO;

public class SkillDto
{
	public string Name { get; set; }
	public string Description { get; set; }
	public string Sort { get; set; }
	private class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<Skill, SkillDto>();
		}
	}
}
