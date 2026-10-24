using ShoesShop.Properties;
using System;
using System.Collections.Generic;

namespace ShoesShop.Models;

public partial class Product
{
    public int IdProducts { get; set; }

    public string ProductName { get; set; } = null!;

    public int IdCategory { get; set; }

    public int IdSubcategory { get; set; }

    public int IdManufacturer { get; set; }

    public string Description { get; set; } = null!;

    public string Composition { get; set; } = null!;

    public string SizeRange { get; set; } = null!;

    public string PhotoUrl { get; set; } = null!;

    public virtual Category IdCategoryNavigation { get; set; } = null!;

    public virtual Manufacturer IdManufacturerNavigation { get; set; } = null!;

    public virtual Subcategory IdSubcategoryNavigation { get; set; } = null!;

    public virtual ICollection<ProductItem> ProductItems { get; set; } = new List<ProductItem>();
}