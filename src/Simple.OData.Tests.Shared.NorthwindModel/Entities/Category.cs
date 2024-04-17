using System.ComponentModel.DataAnnotations;

namespace Simple.OData.Tests.Shared.NorthwindModel.Entities;

public class Category
{
	[Key]
	public int CategoryID { get; set; }
	[Required]
	public string CategoryName { get; set; }
	public string Description { get; set; }
	public byte[] Picture { get; set; }

	public virtual ICollection<Product> Products { get; set; }
}
