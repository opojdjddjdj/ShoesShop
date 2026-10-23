using System;
using System.Collections.Generic;

namespace ShoesShop.Models;

public partial class Manufacturer
{
    public int IdManufacturers { get; set; }

    public string ManufacturerName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
