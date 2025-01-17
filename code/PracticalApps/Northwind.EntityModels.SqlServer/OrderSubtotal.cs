﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.EntityModels;

[Keyless]
public partial class OrderSubtotal
{
	public int OrderId { get; set; }

	[Column(TypeName = "money")]
	public decimal? Subtotal { get; set; }
}