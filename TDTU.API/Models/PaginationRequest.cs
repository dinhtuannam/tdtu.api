namespace TDTU.API.Models;

public class PaginationRequest : BaseRequest
{
	public int PageIndex { get; init; }
	public int PageSize { get; init; }
	
}
