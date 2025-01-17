﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.EntityModels;

[Keyless]
public partial class Territory
{
	[Column(TypeName = "nvarchar] (20")]
	[Required]
	public string TerritoryId { get; set; } = null!;

	[Column(TypeName = "nchar] (50")]
	[Required]
	public string TerritoryDescription { get; set; } = null!;

	[Column(TypeName = "INT")]
	public int RegionId { get; set; }
}