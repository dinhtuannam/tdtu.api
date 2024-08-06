using TDTU.API.Dtos.RegularJobDTO;
using TDTU.API.Models.RegularJobModel;

namespace TDTU.API.Interfaces;

public interface IRegularJobService
{
	Task<PaginatedList<RegularJobDto>> GetPagination(PaginationRequest request);
	Task<List<RegularJobDto>> GetAll(BaseRequest request);
	Task<List<RegularJobDto>> GetFilter(FilterRequest request);
	Task<RegularJobDto> GetById(Guid id);
	Task<bool> DeleteByIds(DeleteRequest request);
	Task<RegularJobDto> Add(RegularJobAddOrUpdate request);
	Task<RegularJobDto> Update(RegularJobAddOrUpdate request);
}
