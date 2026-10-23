using System;
using System.Collections.Generic;

namespace ShoesShop.Models;

public partial class Size
{
    public int IdSizes { get; set; }

    public decimal Size1 { get; set; }

    public virtual ICollection<ProductItem> ProductItems { get; set; } = new List<ProductItem>();
}
