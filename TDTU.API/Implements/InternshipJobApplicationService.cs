using AutoMapper;

namespace TDTU.API.Implements;

public class InternshipJobApplicationService : IInternshipJobApplicationService
{
	private readonly IDataContext _context;
	private readonly IMapper _mapper;
	public InternshipJobApplicationService(IDataContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	private async Task<InternshipTerm?> GetCurrentTerm(Guid? id = null, Guid? user = null)
	{
		DateTime now = DateTime.Now;
		InternshipTerm? term = null;
		var query = _context.InternshipTerms
							.Where(s => s.StartDate < now && now > s.EndDate && s.IsExpired != true)
							.AsNoTracking();

		if (id != null && id != Guid.Empty)
		{
			query = query.Where(s => s.Id == id);
		}

		term = await query.FirstOrDefaultAsync();

		if (user != null && user != Guid.Empty && term != null)
		{
			var valid = await _context.InternshipRegistrations
							  .Where(s => s.InternshipTermId == term.Id && s.StudentId == user)
							  .FirstOrDefaultAsync();

			if (valid == null) return null;
		}

		return term;
	}

	/*public async Task<RegularJobApplicationDto> SetStatus(RegularJobSetStatusRequest request)
	{
		var application = await FindAsync(request.Id);
		var company = await FindCompany(request.CompanyId);

		if (application.Job!.CompanyId != company.Id)
			throw new ApplicationException("Bạn không có quyền thao tác");

		application.StudentId = request.StatusId;

	}*/

	private async Task<Company> FindCompany(Guid id)
	{
		var company = await _context.Companies.FirstOrDefaultAsync(s => s.Id == id);
		if (company == null)
		{
			throw new ApplicationException($"Không tìm thấy công ty với Id: {id}");
		}
		return company;
	}
}
