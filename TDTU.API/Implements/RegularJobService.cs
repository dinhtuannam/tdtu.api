﻿using AutoMapper;
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
		var company = await FindCompany(request.CompanyId);

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

		if (request.Skills.Any())
		{
			var skills = await _context.Skills.Where(s => request.Skills.Contains(s.Id)).ToListAsync();
			job.Skills = skills;
		}

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
		RegularJobDto? data = await _context.RegularJobs.Include(s => s.Skills)
											.Include(s => s.Company).ThenInclude(s => s.User)
											.Where(s => s.Id == id)
											.ProjectTo<RegularJobDto>(_mapper.ConfigurationProvider)
											.FirstOrDefaultAsync();
		return data;
	}

	public async Task<List<RegularJobDto>> GetFilter(FilterRequest request)
	{
		var query = _context.RegularJobs.Include(s => s.Skills)
							.Include(s => s.Company).ThenInclude(s => s.User)
							.ProjectTo<RegularJobDto>(_mapper.ConfigurationProvider).AsNoTracking();

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.Name.ToLower().Contains(text));
		}

		if (request.UserId != null && request.UserId != Guid.Empty)
		{
			query = query.Where(x => x.CompanyId == request.UserId);
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
		var query = _context.RegularJobs.Include(s => s.Skills)
							.Include(s => s.Company).ThenInclude(s => s.User)
							.OrderByDescending(x => x.CreatedDate)
							.ProjectTo<RegularJobDto>(_mapper.ConfigurationProvider);

		if (!string.IsNullOrEmpty(request.TextSearch))
		{
			string text = request.TextSearch.ToLower();
			query = query.Where(x => x.Name.ToLower().Contains(text));
		}

		if(request.UserId != null && request.UserId != Guid.Empty)
		{
			query = query.Where(x => x.CompanyId == request.UserId);
		}

		PaginatedList<RegularJobDto> paging = await query.PaginatedListAsync(request.PageIndex, request.PageSize);
		return paging;
	}

	private async Task<RegularJob> FindAsync(Guid? id)
	{
		if (id == null) throw new ApplicationException($"Không tìm thấy dữ liệu với Id: {id}");

		var job = await _context.RegularJobs.Include(s => s.Skills)
								.Include(s => s.Company).ThenInclude(s => s.User)
								.FirstOrDefaultAsync(s => s.Id == id);

		if (job == null) throw new ApplicationException($"Không tìm thấy dữ liệu với Id: {id}");

		return job;
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

	public async Task<RegularJobDto> Update(RegularJobAddOrUpdate request)
	{
		var company = await FindCompany(request.CompanyId);
		var job = await FindAsync(request.Id);

		if(company.Id != job.Id) throw new ApplicationException($"Bạn không đủ quyền thao tác");

		job.Name = request.Name;
		job.SalaryMin = request.SalaryMin;
		job.SalaryMax = request.SalaryMax;
		job.CompanyId = company.Id;
		job.Company = company;
		job.Description = request.Description;
		job.ExpireDate = request.ExpireDate;

		if(job.Skills != null && job.Skills.Any())
		{
			job.Skills.Clear();
		}
		job.Skills = await _context.Skills.Where(s => request.Skills.Contains(s.Id)).ToListAsync();

		_context.RegularJobs.Update(job);
		await _context.SaveChangesAsync();

		return _mapper.Map<RegularJobDto>(job);
	}
}
