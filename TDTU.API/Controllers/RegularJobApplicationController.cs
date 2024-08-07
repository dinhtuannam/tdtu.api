﻿using Microsoft.AspNetCore.Mvc;
using TDTU.API.Dtos.RegularJobApplicationDTO;
using TDTU.API.Models.RegularJobApplicationModel;

namespace TDTU.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RegularJobApplicationController : BaseController
	{
		private readonly IRegularJobApplicationService _service;
		public RegularJobApplicationController(IRegularJobApplicationService service)
		{
			_service = service;
		}

		[HttpPost("apply")]
		public async Task<IActionResult> Apply(RegularJobApplyRequest request)
		{
			var data = await _service.ApplyJob(request);
			var response = Result<RegularJobApplicationDto>.Success(data);
			return Ok(response);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute] Guid id)
		{
			var data = await _service.GetById(id);
			var response = Result<RegularJobApplicationDto>.Success(data);
			return Ok(response);
		}

		[HttpGet("company/pagination")]
		public async Task<IActionResult> PaginationCompany([FromQuery] PaginationRequest request)
		{
			var data = await _service.JobApplications(request);
			var response = Result<PaginatedList<RegularJobApplicationDto>>.Success(data);
			return Ok(response);
		}

		[HttpGet("student/pagination")]
		public async Task<IActionResult> PaginationStudent([FromQuery] PaginationRequest request)
		{
			var data = await _service.UserHistory(request);
			var response = Result<PaginatedList<RegularJobApplicationDto>>.Success(data);
			return Ok(response);
		}

		[HttpDelete]
		public async Task<IActionResult> Delete([FromBody] DeleteRequest request)
		{
			var data = await _service.DeleteByIds(request);
			var response = Result<bool>.Success(data);
			return Ok(response);
		}
	}
}
