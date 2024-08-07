using Microsoft.AspNetCore.Mvc;
using TDTU.API.Dtos.StudentDTO;

namespace TDTU.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StudentController : BaseController
	{
		private readonly IStudentService _studentService;
		public StudentController(IStudentService studentService)
		{
			_studentService = studentService;
		}

		[HttpGet]
		public async Task<IActionResult> All([FromQuery] BaseRequest request)
		{
			var data = await _studentService.GetAll(request);
			var response = Result<List<StudentDto>>.Success(data);
			return Ok(response);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute] Guid id)
		{
			var data = await _studentService.GetById(id);
			var response = Result<StudentDto>.Success(data);
			return Ok(response);
		}

		[HttpGet("filter")]
		public async Task<IActionResult> Filter([FromQuery] FilterRequest request)
		{
			var data = await _studentService.GetFilter(request);
			var response = Result<List<StudentDto>>.Success(data);
			return Ok(response);
		}

		[HttpGet("pagination")]
		public async Task<IActionResult> Pagination([FromQuery] PaginationRequest request)
		{
			var data = await _studentService.GetPagination(request);
			var response = Result<PaginatedList<StudentDto>>.Success(data);
			return Ok(response);
		}

		[HttpDelete]
		public async Task<IActionResult> Delete([FromBody] DeleteRequest request)
		{
			var data = await _studentService.DeleteByIds(request);
			var response = Result<bool>.Success(data);
			return Ok(response);
		}
	}
}
