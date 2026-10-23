using System;
using System.Collections.Generic;

namespace ShoesShop.Models;

public partial class ProductItem
{
    public int IdProductItems { get; set; }

    public int IdProduct { get; set; }

    public int IdSize { get; set; }

    public int CountInStock { get; set; }

    public decimal Price { get; set; }

    public virtual Product IdProductNavigation { get; set; } = null!;

    public virtual Size IdSizeNavigation { get; set; } = null!;

    public virtual ICollection<ProductsOrder> ProductsOrders { get; set; } = new List<ProductsOrder>();
}
