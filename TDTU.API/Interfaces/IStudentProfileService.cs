﻿using TDTU.API.Dtos.StudentDTO;

namespace TDTU.API.Interfaces;

public interface IStudentProfileService
{
	Task<PaginatedList<StudentProfileDto>> GetPagination(PaginationRequest request);
	Task<List<StudentProfileDto>> GetFilter(FilterRequest request);
	Task<StudentProfileDto> Add(IFormFile file, Guid id);
	Task<bool> DeleteByIds(DeleteRequest request);
}
