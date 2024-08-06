﻿using System.ComponentModel.DataAnnotations.Schema;

namespace TDTU.API.Data;

[Table("tb_order_status")]
public class OrderStatus : BaseStatusEntity
{
	public string Name { get; set; }
	public string Description { get; set; }
	public ICollection<InternshipOrder>? Orders { set; get; }
}
