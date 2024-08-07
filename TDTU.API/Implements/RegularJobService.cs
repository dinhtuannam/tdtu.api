using AutoMapper;
using AutoMapper.QueryableExtensions;
using TDTU.API.Dtos.RegularJobDTO;
using TDTU.API.Models.RegularJobModel;

namespace TDTU.API.Implements;

public class RegularJobService : IRegularJobService
{
	private readonly IDataContext _context;
	private readonly IMapper _mapper;
	public RegularJobService(IDataContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<RegularJobDto> Add(RegularJobAddOrUpdate request)
	{
		var company = await _context.Companies.FirstOrDefaultAsync(s => s.Id == request.CompanyId);
		if (company == null) throw new ApplicationException($"Không tìm thấy dữ liệu với Id: {request.CompanyId}");

		var job = new RegularJob()
		{
			CompanyId = company.Id,
			Company = company,
			Name = request.Name,
			SalaryMax = request.SalaryMax,
			SalaryMin = request.SalaryMin,
			ExpireDate = request.ExpireDate,
			Description = request.Description,
			CreatedApplicationUserId = request.CreatedApplicationUserId,
		};
		_context.RegularJobs.Add(job);
		await _context.SaveChangesAsync();
		return _mapper.Map<RegularJobDto>(job);
	}

	public async Task<bool> DeleteByIds(DeleteRequest request)
	{
		if (request.Ids == null) throw new ApplicationException("Không tìm thấy tham số Id.");
		List<Guid> ids = request.Ids.Select(m => Guid.Parse(m)).ToList();
		var query = await _context.RegularJobs.Where(m => ids.Contains(m.Id)).ToListAsync();
		if (query == null || query.Count == 0) throw new ApplicationException($"Không tìm thấy trong dữ liệu có Id: {string.Join(";", request.Ids)}");

		foreach (var item in query)
		{
			item.DeleteFlag = true;
			item.LastModifiedDate = DateTime.Now;
			item.LastModifiedApplicationUserId = request.ApplicationUserId;
		}
		_context.RegularJobs.UpdateRange(query);

		int rows = await _context.SaveChangesAsync();
		return rows > 0;
	}

	public async Task<List<RegularJobDto>> GetAll(BaseRequest request)
	{
		List<RegularJobDto> data = await _context.RegularJobs.Include(s => s.Company).ThenInclude(s => s.User)
											  .ProjectTo<RegularJobDto>(_mapper.ConfigurationProvider)
											  .ToListAsync();
		return data;
	}

	public async Task<RegularJobDto> GetById(Guid id)
	{
		RegularJobDto? data = await _context.RegularJobs.Include(s => s.Company).ThenInclude(s => s.User)
											  .Where(s => s.Id == id)
											  .ProjectTo<RegularJobDto>(_mapper.ConfigurationProvider)
											  .FirstOrDefaultAsync();
		return data;
	}

	public async Task<List<RegularJobDto>> GetFilter(FilterRequest request)
	{
		var query = _context.RegularJobs.Include(s => s.Company).ThenInclude(s => s.User)
							.ProjectTo<RegularJobDto>(_mapper.ConfigurationProvider).AsNoTracking();

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.Name.ToLower().Contains(text));
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

	public async Task<PaginatedList<RegularJobDto>> GetPagination(PaginationRequest request)
	{
		var query = _context.RegularJobs.Include(s => s.Company).ThenInclude(s => s.User)
							.OrderByDescending(x => x.CreatedDate)
							.ProjectTo<RegularJobDto>(_mapper.ConfigurationProvider);

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.Name.ToLower().Contains(text));
		}

		if(request.Id != null && request.Id != Guid.Empty)
		{
			query = query.Where(x => x.Company != null && x.Company.Id == request.Id);
		}

		PaginatedList<RegularJobDto> paging = await query.PaginatedListAsync(request.PageIndex, request.PageSize);
		return paging;
	}

	public async Task<RegularJobDto> Update(RegularJobAddOrUpdate request)
	{
		var company = await _context.Companies.Include(s => s.User).FirstOrDefaultAsync(s => s.Id == request.CompanyId);
		if (company == null) throw new ApplicationException($"Không tìm thấy dữ liệu với Id: {request.CompanyId}");

		var job = await _context.RegularJobs.FirstOrDefaultAsync(s => s.Id == request.Id);
		if (job == null) throw new ApplicationException($"Không tìm thấy dữ liệu với Id: {request.Id}");

		job.Name = request.Name;
		job.SalaryMin = request.SalaryMin;
		job.SalaryMax = request.SalaryMax;
		job.CompanyId = company.Id;
		job.Company = company;
		job.Description = request.Description;
		job.ExpireDate = request.ExpireDate;

		_context.RegularJobs.Update(job);
		await _context.SaveChangesAsync();
		return _mapper.Map<RegularJobDto>(job);
	}
}
