using AutoMapper;
using AutoMapper.QueryableExtensions;
using TDTU.API.Dtos.SkillDTO;
using TDTU.API.Dtos.UserDTO;

namespace TDTU.API.Implements;

public class SkillService : ISkillService
{
	private readonly IDataContext _context;
	private readonly IMapper _mapper;
	public SkillService(IDataContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<bool> DeleteByIds(DeleteRequest request)
	{
		if (request.Ids == null) throw new ApplicationException("Không tìm thấy tham số Id.");
		List<Guid> ids = request.Ids.Select(m => Guid.Parse(m)).ToList();
		var query = await _context.Skills.Where(m => ids.Contains(m.Id)).ToListAsync();
		if (query == null || query.Count == 0) throw new ApplicationException($"Không tìm thấy trong dữ liệu có Id: {string.Join(";", request.Ids)}");

		foreach (var item in query)
		{
			item.DeleteFlag = true;
			item.LastModifiedDate = DateTime.Now;
			item.LastModifiedApplicationUserId = request.ApplicationUserId;
		}
		_context.Skills.UpdateRange(query);

		int rows = await _context.SaveChangesAsync();
		return rows > 0;
	}

	public async Task<List<SkillDto>> GetAll(BaseRequest request)
	{
		List<SkillDto> data = await _context.Skills.OrderBy(s => s.Name)
											.ProjectTo<SkillDto>(_mapper.ConfigurationProvider)
											.ToListAsync();
		return data;
	}

	public async Task<SkillDto> GetById(Guid id)
	{
		SkillDto? data = await _context.Skills
									   .ProjectTo<SkillDto>(_mapper.ConfigurationProvider)
									   .FirstOrDefaultAsync();
		return data;
	}

	public async Task<List<SkillDto>> GetFilter(FilterRequest request)
	{
		var query = _context.Skills.ProjectTo<SkillDto>(_mapper.ConfigurationProvider).AsNoTracking();

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.Name.ToLower().Contains(text) ||
									 x.Description.ToLower().Contains(text));
		}

		if (request.Skip != null)
		{
			query = query.Skip(request.Skip.Value);
		}

		if (request.TotalRecord != null)
		{
			query = query.Take(request.TotalRecord.Value);
		}

		return await query.ToListAsync();
	}

	public async Task<PaginatedList<SkillDto>> GetPagination(PaginationRequest request)
	{
		var query = _context.Skills.Where(m => m.DeleteFlag == false)
								   .OrderByDescending(x => x.Name)
								   .ProjectTo<SkillDto>(_mapper.ConfigurationProvider);

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.Name.ToLower().Contains(text) ||
									 x.Description.ToLower().Contains(text));
		}

		PaginatedList<SkillDto> paging = await query.PaginatedListAsync(request.PageIndex, request.PageSize);
		return paging;
	}
}
