using AutoMapper;
using AutoMapper.QueryableExtensions;
using TDTU.API.Dtos.InternshipJobDTO;
using TDTU.API.Models.InternshipJobModel;

namespace TDTU.API.Implements;

public class InternshipJobService : IInternshipJobService
{
	private readonly IDataContext _context;
	private readonly IMapper _mapper;
	public InternshipJobService(IDataContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	private async Task<InternshipTerm> FindTerm(Guid id)
	{
		DateTime now = DateTime.Now;
		var term = await _context.InternshipTerms
				   .Where(s => s.StartDate < now && now < s.EndDate &&
							   s.IsExpired != true && s.Id == id)
				   .FirstOrDefaultAsync();

		if (term == null)
		{
			throw new ApplicationException("Đợt thực tập hiện không khả dụng");
		}

		return term;
	}

	private async Task<Company> FindCompany(Guid id)
	{
		var company = await _context.Companies.FirstOrDefaultAsync(s => s.Id == id);
		if (company == null)
		{
			throw new ApplicationException($"Không tìm thấy doanh nghiệp với ID: {id}");
		}

		return company;
	}

	public async Task<InternshipJobDto> Add(InternshipJobAddOrUpdate request)
	{
		var company = await FindCompany(request.CompanyId);
		var term = await FindTerm(request.InternshipTermId);

		var job = new InternshipJob()
		{
			CompanyId = company.Id,
			Company = company,
			InternshipTerm = term,
			InternshipTermId = term.Id,
			Name = request.Name,
			Description = request.Description,
			CreatedApplicationUserId = request.CreatedApplicationUserId,
		};

		if (request.Skills.Any())
		{
			var skills = await _context.Skills.Where(s => request.Skills.Contains(s.Id)).ToListAsync();
			job.Skills = skills;
		}

		_context.InternshipJobs.Add(job);
		await _context.SaveChangesAsync();
		return _mapper.Map<InternshipJobDto>(job);
	}

	public async Task<bool> DeleteByIds(DeleteRequest request)
	{
		if (request.Ids == null) throw new ApplicationException("Không tìm thấy tham số Id.");
		List<Guid> ids = request.Ids.Select(m => Guid.Parse(m)).ToList();
		var query = await _context.InternshipJobs.Where(m => ids.Contains(m.Id)).ToListAsync();
		if (query == null || query.Count == 0) throw new ApplicationException($"Không tìm thấy trong dữ liệu có Id: {string.Join(";", request.Ids)}");

		foreach (var item in query)
		{
			item.DeleteFlag = true;
			item.LastModifiedDate = DateTime.Now;
			item.LastModifiedApplicationUserId = request.ApplicationUserId;
		}
		_context.InternshipJobs.UpdateRange(query);

		int rows = await _context.SaveChangesAsync();
		return rows > 0;
	}

	public async Task<List<InternshipJobDto>> GetAll(BaseRequest request)
	{
		List<InternshipJobDto> data = await _context.InternshipJobs.Include(s => s.Company).ThenInclude(s => s.User)
											  .ProjectTo<InternshipJobDto>(_mapper.ConfigurationProvider)
											  .ToListAsync();
		return data;
	}

	public async Task<InternshipJobDto> GetById(Guid id)
	{
		InternshipJobDto? data = await _context.InternshipJobs.Include(s => s.Skills)
											.Include(s => s.Company).ThenInclude(s => s.User)
											.Where(s => s.Id == id)
											.ProjectTo<InternshipJobDto>(_mapper.ConfigurationProvider)
											.FirstOrDefaultAsync();
		return data;
	}

	public async Task<List<InternshipJobDto>> GetFilter(FilterRequest request)
	{
		var query = _context.InternshipJobs.Include(s => s.Skills)
							.Include(s => s.Company).ThenInclude(s => s.User)
							.ProjectTo<InternshipJobDto>(_mapper.ConfigurationProvider).AsNoTracking();

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.Name.ToLower().Contains(text));
		}

		if (request.Id != null && request.Id != Guid.Empty)
		{
			query = query.Where(x => x.CompanyId == request.Id);
		}

		if (request.TermId != null && request.TermId != Guid.Empty)
		{
			query = query.Where(x => x.InternshipTermId == request.TermId);
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

	public async Task<PaginatedList<InternshipJobDto>> GetPagination(PaginationRequest request)
	{
		var query = _context.InternshipJobs.Include(s => s.Skills)
							.Include(s => s.Company).ThenInclude(s => s.User)
							.OrderByDescending(x => x.CreatedDate)
							.ProjectTo<InternshipJobDto>(_mapper.ConfigurationProvider);

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.Name.ToLower().Contains(text));
		}

		if (request.TermId != null && request.TermId != Guid.Empty)
		{
			query = query.Where(x => x.InternshipTermId == request.TermId);
		}

		if (request.Id != null && request.Id != Guid.Empty)
		{
			query = query.Where(x => x.CompanyId == request.Id);
		}

		PaginatedList<InternshipJobDto> paging = await query.PaginatedListAsync(request.PageIndex, request.PageSize);
		return paging;
	}

	private async Task<InternshipJob> FindAsync(Guid? id)
	{
		if (id == null) throw new ApplicationException($"Không tìm thấy dữ liệu với Id");

		var job = await _context.InternshipJobs.Include(s => s.Skills)
								.Include(s => s.Company).ThenInclude(s => s.User)
								.FirstOrDefaultAsync(s => s.Id == id);

		if (job == null) throw new ApplicationException($"Không tìm thấy dữ liệu với Id: {id}");

		return job;
	}

	public async Task<InternshipJobDto> Update(InternshipJobAddOrUpdate request)
	{
		var company = await FindCompany(request.CompanyId);
		var term = await FindTerm(request.InternshipTermId);
		var job = await FindAsync(request.Id);

		if (company.Id != job.Id) throw new ApplicationException($"Bạn không đủ quyền thao tác");

		job.Name = request.Name;
		job.InternshipTerm = term;
		job.InternshipTermId = term.Id;
		job.CompanyId = company.Id;
		job.Company = company;
		job.Description = request.Description;

		if (job.Skills != null && job.Skills.Any())
		{
			job.Skills.Clear();
		}
		job.Skills = await _context.Skills.Where(s => request.Skills.Contains(s.Id)).ToListAsync();

		_context.InternshipJobs.Update(job);
		await _context.SaveChangesAsync();

		return _mapper.Map<InternshipJobDto>(job);
	}
}