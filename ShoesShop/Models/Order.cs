using System;
using System.Collections.Generic;

namespace ShoesShop.Models;

public partial class Order
{
    public int IdOrders { get; set; }

    public DateOnly DateOfOrder { get; set; }

    public int IdUser { get; set; }

    public decimal TotalAmount { get; set; }

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual ICollection<ProductsOrder> ProductsOrders { get; set; } = new List<ProductsOrder>();
}
