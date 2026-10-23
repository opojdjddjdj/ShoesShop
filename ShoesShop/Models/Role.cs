using System;
using System.Collections.Generic;

namespace ShoesShop.Models;

public partial class Role
{
    public int IdRoles { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
