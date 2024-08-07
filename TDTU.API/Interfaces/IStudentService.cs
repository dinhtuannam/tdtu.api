using TDTU.API.Dtos.StudentDTO;

namespace TDTU.API.Interfaces;

public interface IStudentService
{
	Task<PaginatedList<StudentDto>> GetPagination(PaginationRequest request);
	Task<List<StudentDto>> GetAll(BaseRequest request);
	Task<List<StudentDto>> GetFilter(FilterRequest request);
	Task<StudentDto> GetById(Guid id);
	Task<bool> DeleteByIds(DeleteRequest request);
}
