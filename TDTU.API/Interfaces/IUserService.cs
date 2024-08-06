using TDTU.API.Dtos.UserDTO;
using TDTU.API.Models.UserModel;

namespace TDTU.API.Interfaces;

public interface IUserService
{
	Task<LoginDto> Login(LoginModel request);
	Task<PaginatedList<UserDto>> GetPagination(PaginationRequest request);
	Task<List<UserDto>> GetAll(BaseRequest request);
	Task<List<UserDto>> GetFilter(FilterRequest request);
	Task<UserDto> GetById(Guid id);
	Task<bool> DeleteByIds(DeleteRequest request);
}
