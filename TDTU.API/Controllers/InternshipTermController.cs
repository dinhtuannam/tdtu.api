﻿using Microsoft.AspNetCore.Mvc;
using TDTU.API.Dtos.InternshipTermDTO;
using TDTU.API.Models.InternshipTermModel;

namespace TDTU.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class InternshipTermController : BaseController
	{
		private readonly IInternshipTermService _service;
		public InternshipTermController(IInternshipTermService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<IActionResult> All([FromQuery] BaseRequest request)
		{
			var data = await _service.GetAll(request);
			var response = Result<List<InternshipTermDto>>.Success(data);
			return Ok(response);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute] Guid id)
		{
			var data = await _service.GetById(id);
			var response = Result<InternshipTermDto>.Success(data);
			return Ok(response);
		}

		[HttpGet("filter")]
		public async Task<IActionResult> Filter([FromQuery] FilterRequest request)
		{
			var data = await _service.GetFilter(request);
			var response = Result<List<InternshipTermDto>>.Success(data);
			return Ok(response);
		}

		[HttpGet("pagination")]
		public async Task<IActionResult> Pagination([FromQuery] PaginationRequest request)
		{
			var data = await _service.GetPagination(request);
			var response = Result<PaginatedList<InternshipTermDto>>.Success(data);
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
		public async Task<IActionResult> Create([FromBody] InternshipTermAddOrUpdate request)
		{
			var data = await _service.Add(request);
			var response = Result<InternshipTermDto>.Success(data);
			return Ok(response);
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromBody] InternshipTermAddOrUpdate request)
		{
			var data = await _service.Update(request);
			var response = Result<InternshipTermDto>.Success(data);
			return Ok(response);
		}
	}
}
