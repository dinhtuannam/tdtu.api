using Microsoft.AspNetCore.Mvc;
using TDTU.API.Dtos.InternshipJobDTO;
using TDTU.API.Models.InternshipJobModel;

namespace TDTU.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class InternshipJobController : BaseController
	{
		private readonly IInternshipJobService _service;
		public InternshipJobController(IInternshipJobService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<IActionResult> All([FromQuery] BaseRequest request)
		{
			var data = await _service.GetAll(request);
			var response = Result<List<InternshipJobDto>>.Success(data);
			return Ok(response);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute] Guid id)
		{
			var data = await _service.GetById(id);
			var response = Result<InternshipJobDto>.Success(data);
			return Ok(response);
		}

		[HttpGet("filter")]
		public async Task<IActionResult> Filter([FromQuery] FilterRequest request)
		{
			var data = await _service.GetFilter(request);
			var response = Result<List<InternshipJobDto>>.Success(data);
			return Ok(response);
		}

		[HttpGet("pagination")]
		public async Task<IActionResult> Pagination([FromQuery] PaginationRequest request)
		{
			var data = await _service.GetPagination(request);
			var response = Result<PaginatedList<InternshipJobDto>>.Success(data);
			return Ok(response);
		}

		[HttpDelete]
		public async Task<IActionResult> Delete([FromBody] DeleteRequest request)
		{
			var data = await _service.DeleteByIds(request);
			var response = Result<bool>.Success(data);
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] InternshipJobAddOrUpdate request)
		{
			request.CompanyId = GetUserId() ?? Guid.Empty;
			var data = await _service.Add(request);
			var response = Result<InternshipJobDto>.Success(data);
			return Ok(response);
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromBody] InternshipJobAddOrUpdate request)
		{
			request.CompanyId = GetUserId() ?? Guid.Empty;
			var data = await _service.Update(request);
			var response = Result<InternshipJobDto>.Success(data);
			return Ok(response);
		}
	}
}
