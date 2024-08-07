using Microsoft.AspNetCore.Mvc;
using TDTU.API.Dtos.SkillDTO;

namespace TDTU.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SkillController : ControllerBase
	{
		private readonly ISkillService _skillService;
		public SkillController(ISkillService skillService)
		{
			_skillService = skillService;
		}

		[HttpGet]
		public async Task<IActionResult> All([FromQuery] BaseRequest request)
		{
			var data = await _skillService.GetAll(request);
			var response = Result<List<SkillDto>>.Success(data);
			return Ok(response);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute] Guid id)
		{
			var data = await _skillService.GetById(id);
			var response = Result<SkillDto>.Success(data);
			return Ok(response);
		}

		[HttpGet("filter")]
		public async Task<IActionResult> Filter([FromQuery] FilterRequest request)
		{
			var data = await _skillService.GetFilter(request);
			var response = Result<List<SkillDto>>.Success(data);
			return Ok(response);
		}

		[HttpGet("pagination")]
		public async Task<IActionResult> Pagination([FromQuery] PaginationRequest request)
		{
			var data = await _skillService.GetPagination(request);
			var response = Result<PaginatedList<SkillDto>>.Success(data);
			return Ok(response);
		}

		[HttpDelete]
		public async Task<IActionResult> Delete([FromBody] DeleteRequest request)
		{
			var data = await _skillService.DeleteByIds(request);
			var response = Result<bool>.Success(data);
			return Ok(response);
		}
	}
}
