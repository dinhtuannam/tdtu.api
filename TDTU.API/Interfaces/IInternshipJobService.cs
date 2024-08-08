using TDTU.API.Dtos.InternshipJobDTO;
using TDTU.API.Models.InternshipJobModel;

namespace TDTU.API.Interfaces;

public interface IInternshipJobService
{
	Task<PaginatedList<InternshipJobDto>> GetPagination(PaginationRequest request);
	Task<List<InternshipJobDto>> GetAll(BaseRequest request);
	Task<List<InternshipJobDto>> GetFilter(FilterRequest request);
	Task<InternshipJobDto> GetById(Guid id);
	Task<bool> DeleteByIds(DeleteRequest request);
	Task<InternshipJobDto> Add(InternshipJobAddOrUpdate request);
	Task<InternshipJobDto> Update(InternshipJobAddOrUpdate request);
}
