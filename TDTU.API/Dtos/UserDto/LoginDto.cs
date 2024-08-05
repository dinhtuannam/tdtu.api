namespace TDTU.API.Dtos.UserDto;

public class LoginDto
{
	public UserDto User { get; set; }
	public string Token { get; set; }
	public DateTime ValidTo { get; set; }
}
