﻿namespace TDTU.API.Data;

public class Media : BaseEntity
{
	public string? PublicId { get; set; } = string.Empty;
	public string? OriginalName { get; set; } = string.Empty;
	public string? Url { get; set; } = string.Empty;
	public string? Extension { get; set; } = string.Empty;
}
