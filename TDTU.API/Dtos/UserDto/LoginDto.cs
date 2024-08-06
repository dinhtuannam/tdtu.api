namespace TDTU.API.Dtos.UserDTO;

public class LoginDto
{
	public UserDto User { get; set; }
	public string Token { get; set; }
	public DateTime ValidTo { get; set; }
}
