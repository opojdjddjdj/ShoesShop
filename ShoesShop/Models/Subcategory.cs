using System;
using System.Collections.Generic;

namespace ShoesShop.Models;

public partial class Subcategory
{
    public int IdSubcategories { get; set; }

    public string SubcategoryName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
