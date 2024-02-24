using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Northwind.EntityModels;

[Keyless]
public partial class CurrentProductList
{
	public int ProductId { get; set; }

	[StringLength(40)]
	public string ProductName { get; set; } = null!;
}