using AutoMapper;
using AutoMapper.QueryableExtensions;
using TDTU.API.Dtos.StudentDTO;

namespace TDTU.API.Implements;

public class StudentService : IStudentService
{
	private readonly IDataContext _context;
	private readonly IMapper _mapper;
	public StudentService(IDataContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}
	public async Task<bool> DeleteByIds(DeleteRequest request)
	{
		if (request.Ids == null) throw new ApplicationException("Không tìm thấy tham số Id.");
		List<Guid> ids = request.Ids.Select(m => Guid.Parse(m)).ToList();
		var query = await _context.Students.Where(m => ids.Contains(m.Id)).ToListAsync();
		if (query == null || query.Count == 0) throw new ApplicationException($"Không tìm thấy trong dữ liệu có Id: {string.Join(";", request.Ids)}");

		foreach (var item in query)
		{
			item.DeleteFlag = true;
			item.LastModifiedDate = DateTime.Now;
			item.LastModifiedApplicationUserId = request.ApplicationUserId;
		}
		_context.Students.UpdateRange(query);

		int rows = await _context.SaveChangesAsync();
		return rows > 0;
	}

	public async Task<List<StudentDto>> GetAll(BaseRequest request)
	{
		List<StudentDto> data = await _context.Students.Include(s => s.User)
											  .ProjectTo<StudentDto>(_mapper.ConfigurationProvider)
											  .ToListAsync();
		return data;
	}

	public async Task<StudentDto> GetById(Guid id)
	{
		StudentDto? data = await _context.Students.Include(s => s.User)
										 .ProjectTo<StudentDto>(_mapper.ConfigurationProvider)
										 .FirstOrDefaultAsync();
		return data;
	}

	public async Task<List<StudentDto>> GetFilter(FilterRequest request)
	{
		var query = _context.Students.ProjectTo<StudentDto>(_mapper.ConfigurationProvider).AsNoTracking();

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.Phone.ToLower().Contains(text) ||
									 x.Email.ToLower().Contains(text) ||
									 x.FullName.ToLower().Contains(text) );
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

	public async Task<PaginatedList<StudentDto>> GetPagination(PaginationRequest request)
	{
		var query = _context.Students.Include(s => s.User)
									 .OrderByDescending(x => x.CreatedDate)
									 .ProjectTo<StudentDto>(_mapper.ConfigurationProvider);

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.Email.ToLower().Contains(text) ||
									 x.Phone.ToLower().Contains(text));
		}

		PaginatedList<StudentDto> paging = await query.PaginatedListAsync(request.PageIndex, request.PageSize);
		return paging;
	}
}
