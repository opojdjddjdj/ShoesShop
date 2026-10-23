using System;
using System.Collections.Generic;

namespace ShoesShop.Models;

public partial class ProductsOrder
{
    public int IdProductsOrders { get; set; }

    public int IdProductItem { get; set; }

    public int IdOrder { get; set; }

    public int Quantity { get; set; }

    public virtual Order IdOrderNavigation { get; set; } = null!;

    public virtual ProductItem IdProductItemNavigation { get; set; } = null!;
}
