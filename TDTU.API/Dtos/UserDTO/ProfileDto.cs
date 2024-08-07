using AutoMapper;

namespace TDTU.API.Dtos.UserDTO;

public class ProfileDto : UserDto
{
	public Guid? TermId { get; set; }
	public Guid? RegistrationId { get; set; }
	private class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<User, ProfileDto>();
		}
	}
}
