using System;
using System.Collections.Generic;

namespace ShoesShop.Models;

public partial class User
{
    public int IdUsers { get; set; }

    public int IdRole { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public string Login { get; set; } = null!;

    public virtual Role IdRoleNavigation { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
