namespace TDTU.API.Models;

public class BaseRequest 
{
	public Guid? Id { get; set; }
	public string? TextSearch { get; set; }
	public string? OrderCol { get; set; }
	public string? OrderDir { get; set; }
	public string? Status { get; set; }
	public int? Month { get; set; }
	public int? Year { get; set; }
	public DateTime? Time { get; set; }
}

public static class OrderDir
{
	public static string Desc { get; } = nameof(Desc);
	public static string Asc { get; } = nameof(Asc);
}